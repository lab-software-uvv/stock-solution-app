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
import {
    ArrowCycle,
    ArrowForwardThickFill,
    Cross,
    Pencil,
    PeopleMultiple,
    Save,
    ShippingBoxV1,
    Tag,
    TrashCan,
} from "akar-icons";

//settings
const columns = [
    { field: "id", headerName: "id", width: 25 },
    { field: "name", headerName: "Nome", width: 200 },
    { field: "role", headerName: "Cargo", width: 400 },
];

const Employees = ({ user, setAuth }) => {
    const [popupOn, setPopupOn] = useState(false);
    const [popup, setPopup] = useState(<></>);

    const [triggerChangePage, setTriggerChangePage] = useState(true);

    const [usersList, setUsersList] = useState(null);

    const [selected, setSelected] = useState(null);

    const [email, setEmail] = useState("");

    useEffect(() => {
        loadContent();
    }, []);

    const loadContent = async () => {
        const req = async () => {
            await Requests.get(
                `/users` /*, {
                headers: {
                    authentication: `bearer ${localStorage.getItem("token")}`,
                },
            }*/
            )
                .then((res) => {
                    let aux = [];
                    res.data.forEach((element) =>
                        aux.push({
                            ...element,
                            role:
                                element.role === 1
                                    ? "Admin"
                                    : element.role === 2
                                    ? "Manager"
                                    : "Employee",
                        })
                    );
                    setUsersList(aux);
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
            success: "Funcionários carregados!",
            error: "Erro, tente novamente mais tarde",
        });
    };

    const handleSave = async () => {
        let req = async () => {};

        let statuscode;
        let errMsg = "";

        let obj = {
            email: email,
        };

        req = async () => {
            await Requests.post(`/invites`, obj)
                .then((res) => {
                    statuscode = res.status;
                    clearForm();
                    setPopupOn(true);
                    setPopup(
                        <div style={{ padding: 25 }}>
                            <p>{`Convite: ${res.data.id}`}</p>
                            <RoundedBtn title={"OK"} onClick={() => setPopup(false)}></RoundedBtn>
                        </div>
                    );
                })
                .catch((err) => {
                    console.log(err);
                    throw Error;
                });
        };

        switch (statuscode) {
            case 404:
                errMsg = "Usuário não encontrado";
                throw Error;
            case 400:
                errMsg = "Erro no formulário";
                throw Error;
            case 409:
                errMsg = "O usuário já existe";
                throw Error;

            default:
                break;
        }

        toast.promise(req(), {
            loading: "Salvando...",
            success: "Convite enviado!",
            error: `Erro: ${errMsg}`,
        });
        loadContent();
    };

    const handleDelete = async (selected) => {
        if (selected) {
            const req = async () => {
                await Requests.delete(
                    `/users/${selected.id}` /*, {
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
                success: "Funcionário excluido!",
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
        setEmail("");
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
            <div className="employees-wrapper flex-center flex-column">
                <CrudContainer
                    changePage={triggerChangePage}
                    icon={<PeopleMultiple color="var(--color-darkgrey)" />}
                    title={"Funcionários"}
                    list={
                        <>
                            <div style={{ height: "50vh", width: "60vw" }}>
                                <DataGrid
                                    rows={usersList ? usersList : []}
                                    columns={columns}
                                    onRowClick={(e) => {
                                        handleSelectItem(e);
                                    }}
                                />
                            </div>
                            <div className="flex-row employees-list-btn-wrapper">
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
                                                            <p>Deletar funcionário selecionada?</p>
                                                            <p className="p-subtitle">{`id: ${selected.id}`}</p>
                                                            <p className="p-subtitle">{`Nome: ${selected.name}`}</p>

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
                            </div>
                        </>
                    }
                    form={
                        <>
                            <form className="employees-form flex-column">
                                <div>
                                    <p className="p-text">Email para convite *</p>
                                    <TextInput
                                        type="email"
                                        value={email}
                                        setValue={setEmail}
                                        required={true}
                                        minLength={3}
                                        maxLength={50}
                                        placeholder={"Email"}
                                    ></TextInput>
                                </div>
                                <div></div>
                                <div></div>
                                <div className="employees-form-submit-wrapper flex-row gap-10">
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
                                                        <ArrowForwardThickFill
                                                            size={58}
                                                            color="var(--color-green)"
                                                        />
                                                        <p style={{ marginBottom: 10 }}>
                                                            Deseja enviar o convite?
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
                                        <ArrowForwardThickFill color="white" />
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

export default Employees;
