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
import { ArrowCycle, Cross, Pencil, Save, ShippingBoxV1, Tag, TrashCan } from "akar-icons";

//settings
const columns = [
    { field: "id", headerName: "id", width: 25 },
    { field: "name", headerName: "Nome", width: 200 },
    { field: "description", headerName: "Descrição", width: 400 },
];

const Categories = ({ user, setAuth }) => {
    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [categoriesList, setCategoriesList] = useState([
        { id: 1, Name: `Categoria teste`, Description: `Desc test` },
    ]);

    const [selected, setSelected] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [name, setName] = useState("");
    const [desc, setDesc] = useState("");

    useEffect(() => {
        loadContent();
    }, []);

    const loadContent = async () => {
        const req = async () => {
            await Requests.get(
                `/categories` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
            )
                .then((res) => {
                    setCategoriesList(res.data);
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
            success: "Categorias carregadas!",
            error: "Erro, tente novamente mais tarde",
        });
    };

    const handleSave = async () => {
        let req = async () => {};

        let statuscode;
        let errMsg = "";

        let obj = {
            name: name,
            description: desc,
        };

        if (!isEditing) {
            req = async () => {
                await Requests.post(
                    `/categories`,
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
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };
        } else {
            req = async () => {
                await Requests.put(
                    `/categories/${selected.id}`,
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
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };
        }

        switch (statuscode) {
            case 404:
                errMsg = "Categoria não encontrada";
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
            success: "Categoria salva!",
            error: `Erro: ${errMsg}`,
        });
        loadContent();
    };

    const handleDelete = async (selected) => {
        if (selected) {
            const req = async () => {
                await Requests.delete(
                    `/categories/${selected.id}` /*, {
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
                        console.log(err);
                        throw Error;
                    });
            };

            toast.promise(req(), {
                loading: "Deletando...",
                success: "Categoria excluida!",
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
        setDesc("");
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
            <div className="categories-wrapper flex-center flex-column">
                <CrudContainer
                    changePage={triggerChangePage}
                    icon={<Tag color="var(--color-darkgrey)" />}
                    title={"Categorias"}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={categoriesList}
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
                                                            <p>Deletar categoria selecionada?</p>
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
                                                setName(selected?.name);
                                                setDesc(selected?.description);
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
                            <form className="categories-form flex-column">
                                {isEditing && <p>{`Editando registro ${selected?.id}`}</p>}
                                <div>
                                    <p className="p-text">Nome da categoria *</p>
                                    <TextInput
                                        value={name}
                                        setValue={setName}
                                        required={true}
                                        minLength={3}
                                        maxLength={50}
                                        placeholder={"Nome da categoria"}
                                    ></TextInput>
                                </div>
                                <div>
                                    <p className="p-text">Descrição da categoria</p>
                                    <TextInput
                                        value={desc}
                                        setValue={setDesc}
                                        type={"textarea"}
                                        maxLength={255}
                                        placeholder={"Descreva a categoria (opcional)"}
                                    ></TextInput>
                                </div>
                                <div></div>
                                <div></div>
                                <div className="categories-form-submit-wrapper flex-row gap-10">
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

export default Categories;
