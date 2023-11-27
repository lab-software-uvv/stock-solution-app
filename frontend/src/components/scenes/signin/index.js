import React, { useState, useCallback, useEffect } from "react";
import "./styles.css";
import regex from "../../../utils/regex";
import Requests from "../../../services/requests";

//components
import toast from "react-hot-toast";

import TextInput from "../../ui/text.input";
import TextBtn from "../../ui/text.btn";
import RoundedBtn from "../../ui/rounded.btn";

//assets
import { Envelope, LockOn, EyeOpen, EyeClosed } from "akar-icons";

const SignIn = ({ setCurrentPage, handleMove, setAuth, setUser }) => {
    const [isPassVisible, setIsPassVisible] = useState(false);

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [formFields, setFormFields] = useState({
        email: "var(--color-black)",
        password: "var(--color-black)",
    });

    const handleSignIn = async (e) => {
        e.preventDefault();
        if (validateFields()) {
            const objUser = {
                email: email,
                password: password,
            };

            const req = async () => {
                await Requests.post("/auth/login", {
                    body: objUser,
                    // headers: {
                    //     authentication: `bearer ${JSON.parse(localStorage.getItem("token"))}`,
                    // },
                })
                    .then((res) => {
                        let a = { auth: true, token: res.token };
                        let u = res.user || {};
                        localStorage.setItem("token", res.token);
                        localStorage.setItem("auth", JSON.stringfy(a));
                        localStorage.setItem("user", JSON.stringify(u));
                        setAuth(a);
                        setUser(u);
                        console.log(res);
                    })
                    .catch((err) => {
                        console.log(err);
                        throw Error;
                    });
            };

            toast.promise(
                req(),
                {
                    loading: "Autenticando...",
                    success: "Login efetuado com sucesso!",
                    error: "Erro, tente novamente mais tarde",
                },
                {
                    success: {
                        duration: 2000,
                    },
                }
            );
        }

        localStorage.setItem("token", "514564sa87q6we121x");
        localStorage.setItem("auth", JSON.stringify({ auth: true, token: "514564sa87q6we121x" }));
        localStorage.setItem("user", JSON.stringify({ name: `Fabiano Rabelo`, role: `Gestor` }));
        setUser({ name: `Fabiano Rabelo`, role: `Gestor` });
        setAuth({ auth: true, token: "" });
        console.log("logou");
    };

    const validateFields = () => {
        let errorCount = 0;
        let colors = { ...formFields };

        if (!regex.email.test(email)) {
            colors = { ...colors, email: "var(--color-red)" };
            errorCount++;
        } else {
            colors = { ...colors, email: "var(--color-black)" };
        }

        setFormFields(colors);

        if (errorCount > 0) {
            return false;
        }
        return true;
    };

    const handleKeyPress = useCallback((event) => {
        if (event.key === "Enter") {
            handleSignIn(event);
        }
    }, []);

    useEffect(() => {
        document.addEventListener("keydown", handleKeyPress);
        return () => {
            document.removeEventListener("keydown", handleKeyPress);
        };
    }, [handleKeyPress]);

    return (
        <form className="login-form" onSubmit={handleSignIn}>
            <div className="login-form-gap-2">
                <p className="p-title p-white p-center">Bem-vindo!</p>
                <p className="p-subtitle p-white p-center">Insira seus dados para se conectar</p>
            </div>
            <TextInput
                name={"email"}
                required={true}
                type={"email"}
                value={email}
                setValue={setEmail}
                placeholder={"Email"}
                style={{ borderColor: formFields.email }}
                icoLeft={<Envelope size={18} color={formFields.email} />}
            />
            <TextInput
                name={"password"}
                required={true}
                type={isPassVisible ? "text" : "password"}
                value={password}
                setValue={setPassword}
                placeholder={"Senha"}
                style={{ borderColor: formFields.password }}
                icoLeft={<LockOn size={18} color={formFields.password} />}
                icoRight={
                    isPassVisible ? (
                        <EyeOpen
                            onClick={() => setIsPassVisible(!isPassVisible)}
                            className="button"
                            size={18}
                            color="var(--color-black)"
                        />
                    ) : (
                        <EyeClosed
                            onClick={() => setIsPassVisible(!isPassVisible)}
                            className="button"
                            size={18}
                            color="var(--color-black)"
                        />
                    )
                }
            />
            <div className="login-form-gap-1">
                <TextBtn title={"Esqueci minha senha"} align={"right"} />
            </div>
            <RoundedBtn title={"Entrar"} hierarchy="primary" type="submit" />
            <RoundedBtn
                title={"Cadastrar-se"}
                onClick={() => {
                    handleMove();
                    setCurrentPage(1);
                }}
                hierarchy="secondary"
            />
        </form>
    );
};

export default SignIn;
