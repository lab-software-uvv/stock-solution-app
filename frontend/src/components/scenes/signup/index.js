import React, { useState, useEffect, useCallback } from "react";
import "./styles.css";
import regex from "../../../utils/regex";
import Requests from "../../../services/requests";

//components
import toast from "react-hot-toast";

import TextInput from "../../ui/text.input";
import TextBtn from "../../ui/text.btn";
import RoundedBtn from "../../ui/rounded.btn";

//assets
import { Envelope, LockOn, EyeOpen, Person, CreditCardAlt1, EyeClosed } from "akar-icons";

const SignUp = ({ setCurrentPage, handleMove }) => {
    const path = "?key=";
    const key =
        window.location.href.split(path).length > 0 ? window.location.href.split(path)[1] : null;

    const [isPassVisible, setIsPassVisible] = useState(false);
    const [isPassVisibleC, setIsPassVisibleC] = useState(false);

    const [fullName, setFullName] = useState("");
    const [email, setEmail] = useState("");
    const [CPF, setCPF] = useState("");
    const [birthday, setBirthday] = useState("");
    const [password, setPassword] = useState("");
    const [passwordC, setPasswordC] = useState("");

    const [formFields, setFormFields] = useState({
        fullName: "var(--color-black)",
        email: "var(--color-black)",
        CPF: "var(--color-black)",
        birthday: "var(--color-black)",
        password: "var(--color-black)",
        passwordC: "var(--color-black)",
    });

    useEffect(() => {
        if (window.location.href.split(path).length > 0) verifyKey();
    }, []);

    const verifyKey = async () => {
        return 0;

        const req = async () => {
            await Requests.post(`/invite/validate${path}${key}`)
                .then((res) => {
                    // setEmail(res.email)
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
                loading: "Verificando convite...",
                success: "Convite validado, continue com o cadastro!",
                error: (err) => `Erro: ${err.toString()}`,
            },
            {
                success: {
                    duration: 3500,
                },
            }
        );
    };

    const handleSignUp = async (e) => {
        e.preventDefault();
        if (validateFields()) {
            const objUser = {
                key: key,
                fullName: fullName,
                email: email,
                cpf: CPF,
                birthday: birthday,
                password: password,
            };

            const req = async () => {
                await Requests.post("/auth/register", {
                    body: objUser,
                })
                    .then((res) => {
                        // setAuth({ auth: true, token: null });
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
                    loading: "Efetuando cadastro...",
                    success: "Cadastro efetuado com sucesso!",
                    error: (err) => `Erro: ${err.toString()}`,
                },
                {
                    success: {
                        duration: 2000,
                    },
                }
            );
        }
    };

    const validateFields = () => {
        let errorCount = 0;
        let colors = { ...formFields };

        if (!regex.email.test(email)) {
            toast.error("O email inserido não é um email válido!");
            colors = { ...colors, email: "var(--color-red)" };
            errorCount++;
        } else {
            colors = { ...colors, email: "var(--color-black)" };
        }

        if (!regex.cpf.test(CPF)) {
            toast.error("O CPF inserido não é um CPF válido!");
            colors = { ...colors, CPF: "var(--color-red)" };
            errorCount++;
        } else {
            colors = { ...colors, CPF: "var(--color-black)" };
        }

        if (!regex.password.test(password)) {
            toast.error(
                "A senha inserida deve conter: \n" +
                    "• ao menos 6 caracteres." +
                    "\n" +
                    "• ao menos um dígito." +
                    "\n" +
                    "• ao menos uma letra minúscula." +
                    "\n" +
                    "• ao menos uma letra maiúscula." +
                    "\n",
                {
                    duration: 10000,
                }
            );
            colors = { ...colors, password: "var(--color-red)" };
            errorCount++;
        } else if (password !== passwordC) {
            toast.error("As senhas não coincidem");
            colors = { ...colors, password: "var(--color-red)" };
            colors = { ...colors, passwordC: "var(--color-red)" };
            errorCount++;
        } else {
            colors = { ...colors, password: "var(--color-black)" };
            colors = { ...colors, passwordC: "var(--color-black)" };
        }

        setFormFields(colors);

        if (errorCount > 0) {
            return false;
        }
        return true;
    };

    const handleKeyPress = useCallback((event) => {
        if (event.key === "Enter") {
            handleSignUp(event);
        }
    }, []);

    useEffect(() => {
        document.addEventListener("keydown", handleKeyPress);
        return () => {
            document.removeEventListener("keydown", handleKeyPress);
        };
    }, [handleKeyPress]);

    return (
        <form className="login-form" onSubmit={handleSignUp}>
            <div className="login-form-gap-2">
                <p className="p-title p-white p-center">Crie uma conta</p>
                <p className="p-subtitle p-white p-center">Insira seus dados para se cadastrar</p>
            </div>
            <>
                <TextInput
                    required={true}
                    type={"text"}
                    value={fullName}
                    setValue={setFullName}
                    placeholder={"Insira seu nome completo"}
                    style={{ borderColor: formFields.fullName }}
                    icoLeft={<Person size={18} color={formFields.fullName} />}
                />
                <TextInput
                    required={true}
                    disabled={true}
                    type={"email"}
                    value={email}
                    setValue={setEmail}
                    placeholder={"Email"}
                    icoLeft={<Envelope size={18} color="var(--color-black)" />}
                />
                <TextInput
                    maxLength={11}
                    required={true}
                    type={"text"}
                    value={CPF}
                    setValue={setCPF}
                    placeholder={"Insira seu CPF (apenas números)"}
                    style={{ borderColor: formFields.CPF }}
                    icoLeft={<CreditCardAlt1 size={18} color={formFields.CPF} />}
                />
                <TextInput
                    required={true}
                    textIco={true}
                    type={"date"}
                    value={birthday}
                    setValue={setBirthday}
                    style={{ borderColor: formFields.birthday }}
                    icoLeft={<p className="p-subtitle p-bold">Data de nascimento</p>}
                />
                <TextInput
                    required={true}
                    type={isPassVisible ? "text" : "password"}
                    value={password}
                    setValue={setPassword}
                    placeholder={"Insira sua senha"}
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
                <TextInput
                    required={true}
                    type={isPassVisibleC ? "text" : "password"}
                    value={passwordC}
                    setValue={setPasswordC}
                    placeholder={"Repita a senha"}
                    style={{ borderColor: formFields.passwordC }}
                    icoLeft={<LockOn size={18} color={formFields.passwordC} />}
                    icoRight={
                        isPassVisibleC ? (
                            <EyeOpen
                                onClick={() => setIsPassVisibleC(!isPassVisibleC)}
                                className="button"
                                size={18}
                                color="var(--color-black)"
                            />
                        ) : (
                            <EyeClosed
                                onClick={() => setIsPassVisibleC(!isPassVisibleC)}
                                className="button"
                                size={18}
                                color="var(--color-black)"
                            />
                        )
                    }
                />
            </>
            <div className="login-form-gap-1">
                <TextBtn
                    title={"Já possuo uma conta"}
                    onClick={() => {
                        handleMove();
                        setCurrentPage(0);
                    }}
                    align={"right"}
                />
            </div>
            <RoundedBtn title={"Cadastrar-se"} hierarchy="primary" type="submit" />
        </form>
    );
};

export default SignUp;
