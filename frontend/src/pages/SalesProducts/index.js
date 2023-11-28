import React, { useState, useEffect } from "react";
import "./styles.css";
import Requests from "../../services/requests";
import { useNavigate, useParams } from "react-router-dom";

//components
import toast from "react-hot-toast";

import Navigator from "../../components/scenes/navigator";
import CrudContainer from "../../components/scenes/crudcontainer";
import TextInput from "../../components/ui/text.input";
import IconBtn from "../../components/ui/icon.btn";
import RoundedBtn from "../../components/ui/rounded.btn";
import Popup from "../../components/scenes/popup";

import { DataGrid } from "@mui/x-data-grid";

//assets
import {
    ArrowBackThickFill,
    ArrowCounterClockwise,
    ArrowCycle,
    ChevronLeft,
    ChevronRight,
    Cross,
    Pencil,
    Save,
    ShippingBoxV1,
    TrashCan,
    Utensils,
} from "akar-icons";
import ShrinkBtn from "../../components/ui/shrink.btn";

//settings
const columns = [
    { field: "comercialProductId", headerName: "id-Comercial", width: 100 },
    { field: "productId", headerName: "id-Produto", width: 100 },
    { field: "name", headerName: "Nome", width: 200 },
    { field: "quantity", headerName: "Valor", width: 50 },
    { field: "value", headerName: "Valor", width: 150 },
    { field: "comercialProductName", headerName: "Nome-Comercial", width: 100 },
    { field: "productName", headerName: "Nome-Produto", width: 100 },
];

const SalesProducts = ({ user, setAuth }) => {
    const navigate = useNavigate();
    const params = useParams();

    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [saleProducts, setSaleProducts] = useState(null);

    const [comercialList, setComercialList] = useState(null);
    const [productsList, setProductsList] = useState(null);

    const [selected, setSelected] = useState(null);

    //form fields
    const [productId, setProductId] = useState(0);
    const [comercialProductId, setComercialProductId] = useState(0);
    const [saleId, setSaleId] = useState(params.id);
    const [quantity, setQuantity] = useState(0);
    const [value, setValue] = useState(0);

    const [pEnable, setpEnable] = useState(true);
    const [cEnable, setcEnable] = useState(false);

    useEffect(() => {
        loadContent();
        reqComercialList();
        reqProductsList();
    }, []);

    const loadContent = async () => {
        let content = [];
        try {
            const req = async () => {
                await Requests.get(
                    `/sales/${params.id}/products` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
                )
                    .then(async (res) => {
                        res.data.forEach((element) => {
                            content.push({ ...element, name: element.productName });
                        });
                        await Requests.get(`/sales/${params.id}/comercial-products`).then((res) => {
                            res.data.forEach((element) => {
                                content.push({ ...element, name: element.comercialProductName });
                            });
                            setSaleProducts(content);
                        });
                        setPopupOn(false);
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };

            toast.promise(req(), {
                loading: "Carregando...",
                success: "Produtos carregados!",
                error: "Erro, tente novamente mais tarde",
            });
        } catch (err) {
            navigate("/comercial-products");
            toast.error("Produto não encontrado");
        }
    };

    const reqComercialList = async () => {
        await Requests.get(
            `/comercial-products` /*, {
            headers: {
                authentication: `bearer ${localStorage.getItem("token")}`,
            },
        }*/
        )
            .then((res) => {
                setComercialList(res.data);
                setComercialProductId(res.data[0].id);
                // console.log(res);
            })
            .catch((err) => {
                // console.log(err);
                throw Error;
            });
    };
    const reqProductsList = async () => {
        await Requests.get(
            `/products` /*, {
            headers: {
                authentication: `bearer ${localStorage.getItem("token")}`,
            },
        }*/
        )
            .then((res) => {
                setProductsList(res.data);
                setProductId(res.data[0].id);
                // console.log(res);
            })
            .catch((err) => {
                // console.log(err);
                throw Error;
            });
    };

    const handleSave = async () => {
        let req = async () => {};

        let statuscode;
        let errMsg = "";

        let obj = {
            // Name: name,
            // Code: code,
            // Price: price,
            // Description: description,
        };

        req = async () => {
            await Requests.post(
                `${
                    pEnable
                        ? `/sales/${params.id}/product/${productId}`
                        : `/sales/${params.id}/comercial-product/${comercialProductId}`
                }`,
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
                    console.log(res);
                })
                .catch((err) => {
                    console.log(err);
                    throw Error;
                });
        };

        switch (statuscode) {
            case 404:
                errMsg = "Produto não encontrada";
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
            success: "Produto salvo!",
            error: `Erro: ${errMsg}`,
        });
    };

    const handleDelete = async (selected) => {
        if (selected) {
            const req = async () => {
                await Requests.delete(
                    pEnable
                        ? `/sales/${params.id}/{saleId}/product/${selected.id}`
                        : `/sales/${params.id}/{saleId}/comercial-product/${selected.id}` /*, {
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        console.log(res);
                        setPopupOn(false);
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };

            toast.promise(req(), {
                loading: "Deletando...",
                success: "Produto excluido!",
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
        setQuantity(0);
        setValue(0);
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
            <div className="c-products-wrapper flex-center flex-column">
                <CrudContainer
                    changePage={triggerChangePage}
                    icon={
                        <ArrowBackThickFill
                            color="var(--color-darkgrey)"
                            onClick={() => navigate("/sales")}
                        />
                    }
                    title={`Produtos em venda`}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={saleProducts ? saleProducts : []}
                                    columns={columns}
                                    onRowClick={(e) => {
                                        handleSelectItem(e);
                                    }}
                                />
                            </div>
                            <div className="flex-row categories-list-btn-wrapper">
                                <IconBtn
                                    onClick={() => {
                                        loadContent();
                                    }}
                                    className=""
                                    backgroundColor={"var(--color-darkgrey)"}
                                >
                                    <ArrowCycle color="var(--color-white)" />
                                </IconBtn>
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
                                                        <p>Deletar produto da venda selecionada?</p>
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
                            </div>
                        </>
                    }
                    form={
                        <>
                            <form className="c-products-form">
                                <div
                                    className="sp-tables-wrapper"
                                    style={{ justifyContent: "space-between" }}
                                >
                                    <div className="flex-row gap-10" style={{ width: "100%" }}>
                                        <div style={{ flex: 1 }}>
                                            <div className="flex-row select-title">
                                                <div
                                                    className="button select-btn flex-center"
                                                    onClick={() => {
                                                        setpEnable(true);
                                                        setcEnable(false);
                                                    }}
                                                >
                                                    {pEnable && "•"}
                                                </div>
                                                <p className="p-text">Produto</p>
                                            </div>
                                            <select
                                                value={productId}
                                                onChange={(e) =>
                                                    setProductId(e.currentTarget.value)
                                                }
                                                disabled={!pEnable}
                                            >
                                                {productsList?.map((element) => {
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
                                        <div style={{ flex: 1 }}>
                                            <div className="flex-row select-title">
                                                <div
                                                    className="button select-btn flex-center"
                                                    onClick={() => {
                                                        setcEnable(true);
                                                        setpEnable(false);
                                                    }}
                                                >
                                                    {cEnable && "•"}
                                                </div>
                                                <p className="p-text">Produto comercial</p>
                                            </div>
                                            <select
                                                value={comercialProductId}
                                                onChange={(e) =>
                                                    setComercialProductId(e.currentTarget.value)
                                                }
                                                disabled={!cEnable}
                                            >
                                                {comercialList?.map((element) => {
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
                                    </div>
                                    <div className="flex-row gap-10">
                                        <div style={{ flex: 3 }}>
                                            <p className="p-text">Quantidade vendida *</p>
                                            <TextInput
                                                value={quantity}
                                                setValue={setQuantity}
                                                required={true}
                                                type="number"
                                                min="1"
                                                placeholder={"0"}
                                            ></TextInput>
                                        </div>
                                        <div style={{ flex: 2 }}>
                                            <p className="p-text">Valor total</p>
                                            <TextInput
                                                value={value}
                                                setValue={setValue}
                                                required={true}
                                                type="number"
                                                min="0"
                                                step="any"
                                                icoLeft={<p className="p-text p-price">R$</p>}
                                                placeholder={"0,00"}
                                            ></TextInput>
                                        </div>
                                    </div>
                                </div>

                                <div className="c-products-form-submit-wrapper flex-row gap-10">
                                    <IconBtn
                                        onClick={() => {
                                            clearForm();
                                        }}
                                        backgroundColor={"var(--color-darkgrey)"}
                                    >
                                        <ArrowCounterClockwise color="white" />
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
                                                            "Deseja salvar suas mudanças?
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

export default SalesProducts;
