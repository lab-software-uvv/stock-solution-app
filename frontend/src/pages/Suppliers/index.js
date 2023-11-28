import React, { useState, useEffect } from "react";
import "./styles.css";
import Requests from "../../services/requests";

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
import { ArrowCycle, Cross, Pencil, Save, ShippingBoxV1, ShoppingBag, TrashCan } from "akar-icons";
import regex from "../../utils/regex";

//settings
const columns = [
    { field: "id", headerName: "id", width: 25 },
    { field: "tradingName", headerName: "Nome fantasia", width: 200 },
    { field: "code", headerName: "Código", width: 200 },
    { field: "cnpj", headerName: "CNPJ", width: 100 },
];

const Suppliers = ({ user, setAuth }) => {
    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [suppliersList, setSuppliersList] = useState([
        { id: 1, TradingName: `Categoria teste`, CNPJ: `Desc test`, Code: "teste" },
    ]);

    const [selected, setSelected] = useState(null);
    const [isEditing, setIsEditing] = useState(false);

    //form fields
    const [code, setCode] = useState("");
    const [tradingName, setTradingName] = useState("");
    const [CNPJ, setCNPJ] = useState("");

    useEffect(() => {
        loadContent();
    }, []);

    const loadContent = async () => {
        const req = async () => {
            await Requests.get(
                `/suppliers` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
            )
                .then((res) => {
                    setSuppliersList(res.data);
                    setPopupOn(false);
                    // console.log(res);
                })
                .catch((err) => {
                    // console.log(err);
                    throw Error;
                });
        };

        toast.promise(req(), {
            loading: "Carregando...",
            success: "Fornecedores carregados!",
            error: "Erro, tente novamente mais tarde",
        });
    };

    const handleSave = async () => {
        let req = async () => {};

        if (!regex.cnpj.test(CNPJ)) {
            toast.error("Não é um CNPJ válido");
            return;
        }

        let statuscode;
        let errMsg = "";

        let obj = {
            Code: code,
            TradingName: tradingName,
            CNPJ: CNPJ,
        };

        if (!isEditing) {
            req = async () => {
                await Requests.post(
                    `/suppliers`,
                    obj /*{
                    body: obj,
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        statuscode = res.status;
                        setPopupOn(false);
                        clearForm();
                        // console.log(res);
                    })
                    .catch((err) => {
                        // console.log(err);
                        throw Error;
                    });
            };
        } else {
            req = async () => {
                await Requests.put(
                    `/suppliers/${selected.id}`,
                    obj /*{
                    body: obj,
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        statuscode = res.status;
                        setPopupOn(false);
                        clearForm();
                        // console.log(res);
                    })
                    .catch((err) => {
                        // console.log(err);
                        throw Error;
                    });
            };
        }

        switch (statuscode) {
            case 404:
                errMsg = "Fornecedor não encontrado";
                throw Error;
            case 400:
                errMsg = "Erro no formulário";
                throw Error;
            case 409:
                errMsg = "O item já existe";
                throw Error;

            default:
                break;
        }

        toast.promise(req(), {
            loading: "Salvando...",
            success: "Fornecedor salvo!",
            error: `Erro: ${errMsg}`,
        });

        loadContent();
    };

    const handleDelete = async (selected) => {
        if (selected) {
            const req = async () => {
                await Requests.delete(
                    `/suppliers/${selected.id}` /*, {
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        // console.log(res);
                        setPopupOn(false);
                        loadContent();
                    })
                    .catch((err) => {
                        // console.log(err);
                        throw Error;
                    });
            };

            toast.promise(req(), {
                loading: "Deletando...",
                success: "Fornecedor deletado!",
                error: "Erro, tente novamente mais tarde!",
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
        setCode("");
        setTradingName("");
        setCNPJ("");
        setIsEditing(false);
        setSelected(null);
    };

    const List = () => {
        console.log("create");
    };

    const Form = () => {
        console.log("create form");
    };

    return (
        <Navigator user={user} setAuth={setAuth}>
            {popupOn && <Popup>{popup}</Popup>}
            <div className="suppliers-wrapper flex-center flex-column">
                <CrudContainer
                    changePage={triggerChangePage}
                    icon={<ShoppingBag color="var(--color-darkgrey)" />}
                    title={"Fornecedores"}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={suppliersList ? suppliersList : []}
                                    columns={columns}
                                    onRowClick={(e) => {
                                        handleSelectItem(e);
                                    }}
                                />
                            </div>
                            <div className="flex-row suppliers-list-btn-wrapper">
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
                                                            <p>Deletar fornecedor selecionado?</p>
                                                            <p className="p-subtitle">{`id: ${selected.id}`}</p>
                                                            <p className="p-subtitle">{`Nome: ${selected.tradingName}`}</p>
                                                            <p
                                                                className="p-subtitle"
                                                                style={{ marginBottom: 10 }}
                                                            >
                                                                {`Código: ${selected.code}`}
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
                                                setCode(selected?.code);
                                                setCNPJ(selected?.cnpj);
                                                setTradingName(selected?.tradingName);
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
                            <form className="suppliers-form flex-column">
                                {isEditing && <p>{`Editando registro ${selected?.id}`}</p>}
                                <div>
                                    <p className="p-text">Nome fantasia *</p>
                                    <TextInput
                                        value={tradingName}
                                        setValue={setTradingName}
                                        required={true}
                                        minLength={3}
                                        maxLength={100}
                                        placeholder={"Nome do fornecedor"}
                                    ></TextInput>
                                </div>
                                <div>
                                    <p className="p-text">Código do fornecedor</p>
                                    <TextInput
                                        value={code}
                                        setValue={setCode}
                                        required={true}
                                        minLength={3}
                                        maxLength={50}
                                        placeholder={"Código do fornecedor"}
                                    ></TextInput>
                                </div>
                                <div>
                                    <p className="p-text">CNPJ</p>
                                    <TextInput
                                        value={CNPJ}
                                        setValue={setCNPJ}
                                        required={true}
                                        minLength={3}
                                        maxLength={50}
                                        pattern={regex.cnpj}
                                        placeholder={"CNPJ"}
                                    ></TextInput>
                                </div>
                                <div></div>
                                <div></div>
                                <div className="suppliers-form-submit-wrapper flex-row gap-10">
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

export default Suppliers;
