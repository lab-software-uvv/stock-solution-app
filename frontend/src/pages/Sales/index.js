import React, { useState, useEffect } from "react";
import "./styles.css";
import Requests from "../../services/requests";
import { useNavigate } from "react-router-dom";

//components
import Navigator from "../../components/scenes/navigator";
import CrudContainer from "../../components/scenes/crudcontainer";
import TextInput from "../../components/ui/text.input";
import IconBtn from "../../components/ui/icon.btn";
import Popup from "../../components/scenes/popup";
import RoundedBtn from "../../components/ui/rounded.btn";

import toast from "react-hot-toast";
import { DataGrid } from "@mui/x-data-grid";

//assets
import {
    ArrowCycle,
    Check,
    Cross,
    Money,
    Pencil,
    Save,
    ShippingBoxV1,
    Tag,
    TrashCan,
} from "akar-icons";
import ShrinkBtn from "../../components/ui/shrink.btn";
import getMensagemErroApi from "../../utils/functions/getMensagemErroApi";

//settings
const columns = [
    { field: "id", headerName: "id", width: 25 },
    { field: "seller", headerName: "Vendedor", width: 200 },
    { field: "sellingDate", headerName: "Data", width: 150 },
    { field: "totalValue", headerName: "Valor", width: 150 },
    { field: "paymentMethod", headerName: "Método", width: 125 },
    { field: "status", headerName: "Status", width: 125 },
    { field: "userId", headerName: "Cod", width: 50 },
];

const Sales = ({ user, setAuth }) => {
    const navigate = useNavigate();

    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [salesList, setSalesList] = useState(null);
    const [users, setUsers] = useState(null);

    const [selected, setSelected] = useState(null);
    const [isEditing, setIsEditing] = useState(false);

    //form fields
    const [sellingDate, setSellingDate] = useState(new Date());
    const [totalValue, setTotalValue] = useState(0);
    const [paymentMethod, setPaymentMethod] = useState("");
    const [status, setStatus] = useState("");
    const [userId, setUserId] = useState(1);

    useEffect(() => {
        loadContent();
        reqUsers();
    }, []);

    const loadContent = async () => {
        const req = async () => {
            await Requests.get(
                `/sales` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
            )
                .then(async (res) => {
                    await reqUsers().then(() => {
                        let aux = [];
                        res.data.forEach((element) => {
                            aux.push({
                                ...element,
                                seller: users?.find((e) => e.id === element.userId).name,
                            });
                        });
                        setSalesList(res.data);
                        setPopupOn(false);
                        console.log(res);
                    });
                })
                .catch((err) => {
                    const msgErro = getMensagemErroApi(err);
                    toast.error(msgErro);
                    throw err;
                });
        };

        toast.promise(req(), {
            loading: "Carregando...",
            success: "Vendas carregadas!",
        });
    };

    const reqUsers = async () => {
        await Requests.get(
            `/users` /*, {
            headers: {
                authentication: `bearer ${localStorage.getItem("token")}`,
            },
        }*/
        )
            .then((res) => {
                if (res.status === 404) {
                    return;
                }
                setUsers(res.data);
                if (res.data.length > 0) {
                    setUserId(res.data[0].id);
                }
                console.log(res);
            })
            .catch((err) => {
                // console.log(err);
                throw Error;
            });
    };

    const handleSave = async () => {
        let req = async () => { };

        let statuscode;
        let errMsg = "";

        let obj = {
            sellingDate: sellingDate,
            totalValue: totalValue,
            paymentMethod: paymentMethod,
            status: status,
            userId: userId,
            // userId: 1,
        };

        if (!isEditing) {
            req = async () => {
                await Requests.post(
                    `/sales`,
                    obj /*{
                    obj,
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    }
                }*/
                )
                    .then((res) => {
                        statuscode = res.status;
                        setPopupOn(false);
                        clearForm();
                        console.log(res);
                        loadContent();
                    })
                    .catch((err) => {
                        const msgErro = getMensagemErroApi(err);
                        toast.error(msgErro);
                        throw err;
                    });
            };
        } else {
            req = async () => {
                await Requests.put(
                    `/sales/${selected.id}`,
                    obj /*{
                    body: obj,
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    }
                }*/
                )
                    .then((res) => {
                        statuscode = res.status;
                        setPopupOn(false);
                        clearForm();
                        console.log(res);
                        loadContent();
                    })
                    .catch((err) => {
                        const msgErro = getMensagemErroApi(err);
                        toast.error(msgErro);
                        throw err;
                    });
            };
        }

        switch (statuscode) {
            case 404:
                errMsg = "Venda não encontrada";
                throw Error;
            case 400:
                errMsg = "Erro no formulário";
                throw Error;
            case 409:
                errMsg = "O item já existe";
                throw Error;

            default:
                errMsg = "Preencha todos os campos e tente novamente";
                break;
        }

        toast.promise(req(), {
            loading: "Salvando...",
            success: "Venda salva!",
        });
    };

    const handleDelete = async (selected) => {
        if (selected) {
            const req = async () => {
                await Requests.delete(
                    `/sales/${selected.id}` /*, {
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        setPopupOn(false);
                        console.log(res);
                        loadContent();
                    })
                    .catch((err) => {
                        const msgErro = getMensagemErroApi(err);
                        toast.error(msgErro);
                        throw err;
                    });
            };

            toast.promise(req(), {
                loading: "Deletando...",
                success: "Venda excluida!",
            });
        }
    };

    const handleCancel = () => {
        if (selected) {
            const req = async () => {
                await Requests.put(
                    `/sales/${selected.id}/cancel` /*, {
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        setPopupOn(false);
                        console.log(res);
                        loadContent();
                    })
                    .catch((err) => {
                        const msgErro = getMensagemErroApi(err);
                        toast.error(msgErro);
                        throw err;
                    });
            };

            toast.promise(req(), {
                loading: "Cancelando venda...",
                success: "Venda cancelada!",
            });
        }
    };

    const handleComplete = () => {
        if (selected) {
            const req = async () => {
                await Requests.put(
                    `/sales/${selected.id}/finish` /*, {
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        setPopupOn(false);
                        console.log(res);
                        loadContent();
                    })
                    .catch((err) => {
                        const msgErro = getMensagemErroApi(err);
                        toast.error(msgErro);
                        throw err;
                    });
            };

            toast.promise(req(), {
                loading: "Finalizando venda...",
                success: "Venda finalizada!"
            });
        }
    };

    const handleSelectItem = (selected) => {
        let item = selected.row;
        if (item) {
            setSelected(item);
        } else {
            setSelected(null);
        }
    };

    const clearForm = () => {
        setSellingDate(new Date());
        setTotalValue(0);
        setPaymentMethod("");
        setStatus("");
        setUserId(1);
        setIsEditing(false);
        setSelected(null);
    };

    const List = () => {
        console.log("create list");
    };

    const Form = () => {
        console.log("create form");
    };

    return (
        <Navigator user={user} setAuth={setAuth}>
            {popupOn && <Popup>{popup}</Popup>}
            <div className="sales-wrapper flex-center flex-column">
                <CrudContainer
                    changePage={triggerChangePage}
                    icon={<Money color="var(--color-darkgrey)" />}
                    title={"Vendas"}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={salesList ? salesList : []}
                                    columns={columns}
                                    onRowClick={(e) => {
                                        handleSelectItem(e);
                                    }}
                                />
                            </div>
                            <div className="flex-row sales-list-btn-wrapper">
                                <IconBtn
                                    onClick={() => {
                                        loadContent();
                                    }}
                                    className=""
                                    backgroundColor={"var(--color-darkgrey)"}
                                >
                                    <ArrowCycle color="var(--color-white)" />
                                </IconBtn>
                                <div className="flex-row  gap-10 ">
                                    <ShrinkBtn
                                        action={() =>
                                            selected ? handleCancel() : toast("Selecione um item!")
                                        }
                                        // action={() => navigate(`/sales/0/products`)}
                                        text={"Cancelar venda"}
                                        backgroundColor={"var(--color-red)"}
                                        mouseOnBg={"var(--color-red)"}
                                        shrink={true}
                                        width={250}
                                    >
                                        <Cross color="white"></Cross>
                                    </ShrinkBtn>
                                    <ShrinkBtn
                                        action={() =>
                                            selected
                                                ? handleComplete()
                                                : toast("Selecione um item!")
                                        }
                                        // action={() => navigate(`/sales/0/products`)}
                                        text={"Concluir venda"}
                                        backgroundColor={"var(--color-green)"}
                                        mouseOnBg={"var(--color-green)"}
                                        shrink={true}
                                        width={250}
                                    >
                                        <Check color="white"></Check>
                                    </ShrinkBtn>
                                    <ShrinkBtn
                                        action={() =>
                                            selected
                                                ? navigate(`/sales/${selected.id}/products`)
                                                : toast("Selecione um item!")
                                        }
                                        // action={() => navigate(`/sales/0/products`)}
                                        text={"Associação de produtos"}
                                        backgroundColor={"var(--color-primary)"}
                                        mouseOnBg={"var(--color-primary)"}
                                        shrink={true}
                                        width={250}
                                    >
                                        <ShippingBoxV1 color="white"></ShippingBoxV1>
                                    </ShrinkBtn>
                                    <IconBtn
                                        onClick={() => {
                                            if (selected) {
                                                setPopup(
                                                    <>
                                                        <div
                                                            className="flex-center flex-column gap-10"
                                                            style={{ padding: 20 }}
                                                        >
                                                            <TrashCan
                                                                size={58}
                                                                color="var(--color-red)"
                                                            />
                                                            <p>Deletar venda selecionada?</p>
                                                            <p className="p-subtitle">{`id: ${selected.id}`}</p>
                                                            <p className="p-subtitle">{`Nome: ${selected.name}`}</p>
                                                            <p
                                                                className="p-subtitle"
                                                                style={{ marginBottom: 10 }}
                                                            >
                                                                {`Descrição: ${selected.description}`}
                                                            </p>
                                                            <RoundedBtn
                                                                onClick={() => {
                                                                    handleDelete(selected);
                                                                }}
                                                                title={"Deletar"}
                                                            />
                                                            <RoundedBtn
                                                                onClick={() => {
                                                                    setPopupOn(false);
                                                                }}
                                                                title={"Voltar"}
                                                                hierarchy="secondary"
                                                            />
                                                        </div>
                                                    </>
                                                );
                                                setPopupOn(true);
                                            }
                                        }}
                                        className=""
                                        backgroundColor={"var(--color-red)"}
                                    >
                                        <TrashCan color="white" />
                                    </IconBtn>
                                    <IconBtn
                                        onClick={() => {
                                            if (selected) {
                                                setSellingDate(selected?.sellingDate.split("T")[0]);
                                                setTotalValue(selected?.totalValue);
                                                setPaymentMethod(selected?.paymentMethod);
                                                setStatus(selected?.status);
                                                setUserId(selected?.userId);
                                                setTriggerChangePage(!triggerChangePage);
                                                setIsEditing(true);
                                            }
                                        }}
                                        className=""
                                        backgroundColor={"var(--color-black)"}
                                    >
                                        <Pencil color="white" />
                                    </IconBtn>
                                </div>
                            </div>
                        </>
                    }
                    form={
                        <>
                            <form className="sales-form flex-column">
                                {isEditing && <p>{`Editando registro ${selected?.id}`}</p>}
                                <div>
                                    <p className="p-text">Vendedor</p>
                                    <select
                                        value={userId}
                                        onChange={(e) => setUserId(e.currentTarget.value)}
                                    >
                                        {users?.map((element) => {
                                            return (
                                                <option
                                                    key={element.id}
                                                    value={element.id}
                                                    label={element.name}
                                                >
                                                    {element.name}
                                                </option>
                                            );
                                        })}
                                    </select>
                                </div>
                                <div>
                                    <p className="p-text">Data da venda *</p>
                                    <TextInput
                                        value={sellingDate}
                                        setValue={setSellingDate}
                                        required={true}
                                        type="date"
                                    ></TextInput>
                                </div>
                                <div>
                                    <p className="p-text">Valor da venda</p>
                                    <TextInput
                                        value={totalValue}
                                        setValue={setTotalValue}
                                        required={true}
                                        type="number"
                                        min="0"
                                        step="any"
                                        icoLeft={<p className="p-text p-price">R$</p>}
                                        placeholder={"0,00"}
                                    ></TextInput>
                                </div>
                                <div>
                                    <p className="p-text">Método de pagamento</p>
                                    <select
                                        value={userId}
                                        onChange={(e) => setUserId(e.currentTarget.value)}
                                    >
                                        <option value={"Débito"}>{"Débito"}</option>
                                        <option value={"Crédito"}>{"Crédito"}</option>
                                        <option value={"Dinheiro"}>{"Dinheiro"}</option>
                                        <option value={"Boleto"}>{"Boleto"}</option>
                                    </select>
                                </div>

                                <div></div>
                                <div></div>
                                <div className="sales-form-submit-wrapper flex-row gap-10">
                                    <IconBtn
                                        onClick={() => {
                                            clearForm();
                                        }}
                                        backgroundColor={"var(--color-darkgrey)"}
                                    >
                                        <Cross color="white" />
                                    </IconBtn>
                                    <IconBtn
                                        onClick={() => {
                                            setPopup(
                                                <>
                                                    <div
                                                        className="flex-center flex-column gap-10"
                                                        style={{ padding: 20 }}
                                                    >
                                                        <Save
                                                            size={58}
                                                            color="var(--color-green)"
                                                        />
                                                        <p style={{ marginBottom: 10 }}>
                                                            {isEditing
                                                                ? "Deseja salvar suas mudanças?"
                                                                : "Deseja confirmar o cadastro?"}
                                                        </p>
                                                        <RoundedBtn
                                                            onClick={() => {
                                                                handleSave();
                                                            }}
                                                            title={"Salvar"}
                                                        />
                                                        <RoundedBtn
                                                            onClick={() => {
                                                                setPopupOn(false);
                                                            }}
                                                            title={"Voltar"}
                                                            hierarchy="secondary"
                                                        />
                                                    </div>
                                                </>
                                            );
                                            setPopupOn(true);
                                        }}
                                        backgroundColor={"var(--color-primary)"}
                                    >
                                        <Save color="white" />
                                    </IconBtn>
                                </div>
                            </form>
                        </>
                    }
                ></CrudContainer>
            </div>
        </Navigator>
    );
};

export default Sales;
