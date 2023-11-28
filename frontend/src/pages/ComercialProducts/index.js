import React, { useState, useEffect } from "react";
import "./styles.css";
import Requests from "../../services/requests";
import { useNavigate } from "react-router-dom";

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
import { ArrowCycle, Cross, Pencil, Save, ShippingBoxV1, TrashCan, Utensils } from "akar-icons";
import ShrinkBtn from "../../components/ui/shrink.btn";

//settings
const columns = [
    { field: "id", headerName: "id", width: 50 },
    { field: "name", headerName: "Nome", width: 100 },
    { field: "code", headerName: "Código", width: 125 },
    { field: "price", headerName: "Valor", width: 75 },
    { field: "description", headerName: "Descrição", width: 250 },
];

const ComercialProducts = ({ user, setAuth }) => {
    const navigate = useNavigate();

    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [comercialProductList, setComercialProductList] = useState([
        {
            id: 1,
            name: "name",
            code: "code",
            price: 20,
            description: "description",
        },
    ]);

    const [categoriesList, setCategoriesList] = useState([]);
    const [suppliersList, setSuppliersList] = useState([]);

    const [selected, setSelected] = useState(null);
    const [isEditing, setIsEditing] = useState(false);

    //form fields
    const [name, setName] = useState("");
    const [code, setCode] = useState("");
    const [quantity, setQuantity] = useState(0);
    const [supplier, setSupplier] = useState(1);
    const [price, setPrice] = useState(0);
    const [aquisitionDate, setAquisitionDate] = useState(new Date());
    const [expirationDate, setExpirationDate] = useState(new Date());
    const [description, setDescription] = useState("");
    const [category, setCategory] = useState(1);

    useEffect(() => {
        loadContent();
        reqCategories();
        reqSuppliers();
    }, []);

    const loadContent = async () => {
        const req = async () => {
            await Requests.get(
                `/comercial-products` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
            )
                .then((res) => {
                    setComercialProductList(res.data);
                    setPopupOn(false);
                    console.log(res);
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
    };

    const reqCategories = async () => {
        await Requests.get(
            `/categories` /*, {
            headers: {
                authentication: `bearer ${localStorage.getItem("token")}`,
            },
        }*/
        )
            .then((res) => {
                setCategoriesList(res.data);

                if (res.data.length > 0) {
                    setCategory(res.data[0].id);
                }
                // console.log(res);
            })
            .catch((err) => {
                // console.log(err);
                throw Error;
            });
    };

    const reqSuppliers = async () => {
        await Requests.get(
            `/suppliers` /*, {
            headers: {
                authentication: `bearer ${localStorage.getItem("token")}`,
            },
        }*/
        )
            .then((res) => {                
                setSuppliersList(res.data);
                
                if (res.data.length > 0) {
                    setSupplier(res.data[0].id);
                }
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

        if (!isEditing) {
            req = async () => {
                await Requests.post(
                    `/comercial-products`,
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
                        console.log(res);
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };
        } else {
            req = async () => {
                await Requests.put(
                    `/comercial-products/${selected.id}`,
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

                        console.log(res);
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };
        }

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

        loadContent();
    };

    const handleDelete = async (selected) => {
        if (selected) {
            const req = async () => {
                await Requests.delete(
                    `/comercial-products/${selected.id}` /*, {
                    headers: {
                        authentication: `bearer ${localStorage.getItem("token")}`,
                    },
                }*/
                )
                    .then((res) => {
                        console.log(res);
                        loadContent();
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
        setName("");
        setCode("");
        setQuantity(0);
        setSupplier("");
        setPrice(0);
        setAquisitionDate(new Date());
        setExpirationDate(new Date());
        setDescription("");
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
            <div className="c-products-wrapper flex-center flex-column">
                <CrudContainer
                    changePage={triggerChangePage}
                    icon={<Utensils color="var(--color-darkgrey)" />}
                    title={"Produtos Comerciais"}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={comercialProductList? comercialProductList : []}
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
                                <div className="flex-row  gap-10 ">
                                    <ShrinkBtn
                                        action={() =>
                                            selected
                                                ? navigate(
                                                      `/comercial-products/${selected.id}/products`
                                                  )
                                                : toast("Selecione um produto comercial!")
                                        }
                                        text={"Associação de produtos"}
                                        backgroundColor={"var(--color-green)"}
                                        mouseOnBg={"var(--color-green)"}
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
                                                            <p>Deletar produto selecionado?</p>
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
                                            console.log(selected);
                                            if (selected) {
                                                setName(selected.name);
                                                setCode(selected.code);
                                                setPrice(selected.price);
                                                setDescription(selected.description);
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
                            {isEditing && <p>{`Editando registro ${selected?.id}`}</p>}
                            <form className="c-products-form">
                                <div
                                    className="flex-row"
                                    style={{ justifyContent: "space-between" }}
                                >
                                    <div>
                                        <p className="p-text">Nome do produto *</p>
                                        <TextInput
                                            value={name}
                                            setValue={setName}
                                            required={true}
                                            placeholder={"Nome do produto"}
                                        ></TextInput>
                                    </div>
                                    <div>
                                        <p className="p-text">Código do produto *</p>
                                        <TextInput
                                            value={code}
                                            setValue={setCode}
                                            required={true}
                                            placeholder={"Código do produto"}
                                        ></TextInput>
                                    </div>
                                    <div>
                                        <p className="p-text">Valor *</p>
                                        <TextInput
                                            value={price}
                                            setValue={setPrice}
                                            required={true}
                                            type="number"
                                            min="1"
                                            step="any"
                                            icoLeft={<p className="p-text p-price">R$</p>}
                                            placeholder={"0,00"}
                                        ></TextInput>
                                    </div>
                                </div>

                                <div>
                                    <p className="p-text">Descrição</p>
                                    <TextInput
                                        value={description}
                                        setValue={setDescription}
                                        required={true}
                                        type="textarea"
                                        placeholder={"Descrição do produto"}
                                    ></TextInput>
                                </div>
                                <div className="c-products-form-submit-wrapper flex-row gap-10">
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

export default ComercialProducts;
