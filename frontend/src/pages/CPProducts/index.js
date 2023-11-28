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
    { field: "name", headerName: "Nome", width: 100 },
    { field: "code", headerName: "Código", width: 125 },
    { field: "price", headerName: "Valor", width: 75 },
    { field: "description", headerName: "Descrição", width: 250 },
];

const CPProducts = ({ user, setAuth }) => {
    const navigate = useNavigate();
    const params = useParams();

    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [comercialProduct, setComercialProduct] = useState(null);

    const [productsSelected, setProductsSelected] = useState([]);
    const [productsList, setProductsList] = useState([]);

    const [selected, setSelected] = useState(null);

    //form fields
    const [name, setName] = useState("");
    const [code, setCode] = useState("");
    const [price, setPrice] = useState(0);
    const [description, setDescription] = useState("");

    useEffect(() => {
        loadContent();
        reqProductsList();
    }, []);

    const loadContent = async () => {
        try {
            const req = async () => {
                console.log(params);
                await Requests.get(
                    `/comercial-products/${params.id}` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
                )
                    .then(async (res) => {
                        let obj = res?.data
                        setComercialProduct(obj);
                        setPopupOn(false);

                        // await Requests.get(`/comercial-products/${params.id}/products`)
                        //     .then((res) => {
                        //         if (res.status === 404) {
                        //             return;
                        //         }
                        //         reqProductsList(res.data);
                        //     })
                        //     .catch((err) => {});
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

    const reqProductsList = async (filterList) => {
        await Requests.get(
            `comercial-products/${params.id}/products` /*, {
            headers: {
                authentication: `bearer ${localStorage.getItem("token")}`,
            },
        }*/
        )
            .then((res) => {
                setProductsList(res.data ? res.data.filter((elemento) => !filterList.includes(elemento)) : []);
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
            Name: name,
            Code: code,
            Price: price,
            Description: description,
        };

        req = async () => {
            await Requests.put(
                `/comercial-products/${params.id}/products/${selected.id}`,
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

    // const handleDelete = async (selected) => {
    //     if (selected) {
    //         const req = async () => {
    //             await Requests.delete(
    //                 `/comercial-products/${selected.id}` /*, {
    //                 headers: {
    //                     authentication: `bearer ${localStorage.getItem("token")}`,
    //                 },
    //             }*/
    //             )
    //                 .then((res) => {
    //                     console.log(res);
    //                     setPopupOn(false);
    //                 })
    //                 .catch((err) => {
    //                     console.log(err);
    //                     throw Error;
    //                 });
    //         };

    //         toast.promise(req(), {
    //             loading: "Deletando...",
    //             success: "Produto excluido!",
    //             error: "Erro, tente novamente mais tarde!",
    //         });
    //     }
    // };

    const handleSelectItem = (selected) => {
        let item = selected.row;
        if (item) {
            setSelected(item);
        } else {
            setSelected(null);
        }
    };

    const clearForm = () => {
        setName("");
        setCode("");
        setPrice(0);
        setDescription("");
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
                        comercialProduct ? comercialProduct.name : "Produto Comercial"
                    }`}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                            <DataGrid
                                rows={comercialProduct && comercialProduct.products ? comercialProduct.products : []}
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
                                        reqProductsList();
                                    }}
                                    className=""
                                    backgroundColor={"var(--color-darkgrey)"}
                                >
                                    <ArrowCycle color="var(--color-white)" />
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
                                    <div style={{ flex: 1 }}>
                                        <div className="flex-row">
                                            <div style={{ flex: 2 }}>Produto</div>
                                            <div style={{ flex: 1 }}>Qtd</div>
                                        </div>
                                        <div style={{ overflowY: "auto", height: "50vh" }}>
                                            {productsSelected?.map((e, i) => {
                                                return (
                                                    <div className="flex-row cpp-table-item">
                                                        <div style={{ flex: 3 }}>
                                                            <p>{e.name}</p>
                                                        </div>
                                                        <div style={{ flex: 1 }}>
                                                            <input
                                                                style={{
                                                                    width: 50,
                                                                    textAlign: "center",
                                                                }}
                                                                placeholder="0"
                                                                type="number"
                                                                min={1}
                                                                value={e.qtd}
                                                            ></input>
                                                        </div>
                                                        <div
                                                            style={{ flex: 1 }}
                                                            className="cpp-move-btn-r flex-center button"
                                                            onClick={() => {
                                                                setProductsList([
                                                                    ...productsList,
                                                                    e,
                                                                ]);
                                                                setProductsSelected(
                                                                    productsSelected.filter(
                                                                        (p) => p !== e
                                                                    )
                                                                );
                                                            }}
                                                        >
                                                            <ChevronRight color="white"></ChevronRight>
                                                        </div>
                                                    </div>
                                                );
                                            })}
                                        </div>
                                    </div>
                                    <div style={{ flex: 1 }}>
                                        <div className="flex-row">
                                            <div>Produto</div>
                                        </div>
                                        <div style={{ overflowY: "auto", height: "50vh" }}>
                                            {productsList?.map((e, i) => {
                                                return (
                                                    <div className="flex-row cpp-table-item">
                                                        <div
                                                            className="cpp-move-btn-g flex-center button"
                                                            onClick={() => {
                                                                setProductsSelected([
                                                                    ...productsSelected,
                                                                    e,
                                                                ]);
                                                                setProductsList(
                                                                    productsList.filter(
                                                                        (p) => p !== e
                                                                    )
                                                                );
                                                            }}
                                                        >
                                                            <ChevronLeft color="white"></ChevronLeft>
                                                        </div>
                                                        <div style={{ flex: 3 }}>
                                                            <p>{e.name}</p>
                                                        </div>
                                                    </div>
                                                );
                                            })}
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
