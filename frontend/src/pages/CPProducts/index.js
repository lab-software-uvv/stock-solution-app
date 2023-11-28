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
    { field: "id", headerName: "id", width: 50 },
    { field: "productId", headerName: "Código", width: 100 },
    { field: "productName", headerName: "Nome", width: 250 },
    { field: "quantity", headerName: "Quantidade", width: 100 },
];

const CPProducts = ({ user, setAuth }) => {
    const navigate = useNavigate();
    const params = useParams();

    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [comercialProduct, setComercialProduct] = useState(null);
    const [comercialList, setComercialList] = useState(null);
    const [productsList, setProductsList] = useState([]);

    const [selected, setSelected] = useState(null);

    //form fields
    const [productId, setProductId] = useState(0);
    const [quantity, setQuantity] = useState(1);

    useEffect(() => {
        loadContent();
        reqProductsList();
    }, []);

    const loadContent = async () => {
        try {
            const req = async () => {
                await Requests.get(
                    `/comercial-products/${params.id}` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
                )
                    .then(async (res) => {
                        setComercialProduct(res.data);
                        setPopupOn(false);

                        await reqProductsList();

                        await Requests.get(`/comercial-products/${params.id}/products`)
                            .then((res) => {
                                if (res.status === 404) {
                                    return;
                                }
                                let aux = [];
                                res.data.forEach((element) => {
                                    aux.push({
                                        id: element.id,
                                        comercialProductId: element.comercialProductId,
                                        productId: element.productId,
                                        productName: productsList.find(
                                            (e) => e.id === element.productId
                                        ).name,
                                        quantity: element.quantity,
                                    });
                                });
                                setComercialList(
                                    aux.filter((e) => e.comercialProductId === parseInt(params.id))
                                );
                            })
                            .catch((err) => {});
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
            productId: productId,
            quantity: quantity,
        };

        req = async () => {
            await Requests.post(
                `/comercial-products/${params.id}/products`,
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
                    clearForm()
                    loadContent()
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
                errMsg = "Tente novamente mais tarde";
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
                    `/comercial-products/${params.id}/products/${selected.id}` /*, {
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
        loadContent();
    };

    const handleSelectItem = (selected) => {
        let item = selected.row;
        if (item) {
            setSelected(item);
        } else {
            setSelected(null);
        }
        console.log(item);
    };

    const clearForm = () => {
        setProductId(0);
        setQuantity(1);
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
                            onClick={() => navigate("/comercial-products")}
                        />
                    }
                    title={`Produtos em ${
                        comercialProduct && comercialProduct.name
                            ? comercialProduct.name
                            : "Produto Comercial"
                    }`}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={comercialList ? comercialList : []}
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
                                                        <p className="p-subtitle">{`Nome: ${selected.productName}`}</p>
                                                        <p
                                                            className="p-subtitle"
                                                            style={{ marginBottom: 10 }}
                                                        >
                                                            {`Quantidade: ${selected.quantity}`}
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
                                    className="flex-row cpp-tables-wrapper"
                                    style={{ justifyContent: "space-between" }}
                                >
                                    <div className="flex-row gap-10" style={{ width: "100%" }}>
                                        <div style={{ flex: 1 }}>
                                            <p className="p-text">Produto</p>
                                            <select
                                                value={productId}
                                                onChange={(e) =>
                                                    setProductId(e.currentTarget.value)
                                                }
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
                                    </div>
                                    <div className="flex-row gap-10">
                                        <div style={{ flex: 3 }}>
                                            <p className="p-text">Quantidade *</p>
                                            <TextInput
                                                value={quantity}
                                                setValue={setQuantity}
                                                required={true}
                                                type="number"
                                                min="1"
                                                placeholder={"0"}
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

export default CPProducts;
