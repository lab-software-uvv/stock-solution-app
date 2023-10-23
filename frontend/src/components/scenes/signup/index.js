import React, { useState } from "react";
import "./styles.css";

//components
import TextInput from "../../ui/text.input";
import TextBtn from "../../ui/text.btn";
import RoundedBtn from "../../ui/rounded.btn";

//assets
import { Envelope, LockOn, EyeOpen, Key } from "akar-icons";

const SignUp = ({ setCurrentPage, handleMove }) => {
    const [isPassVisible, setIsPassVisible] = useState(false);
    const [isPassVisibleC, setIsPassVisibleC] = useState(false);
    const [isKeyVisible, setIsKeyVisible] = useState(false);

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [passwordC, setPasswordC] = useState("");
    const [key, setKey] = useState("");

    return (
        <form className="login-form">
            <div className="login-form-gap-2">
                <p className="p-title p-white p-center">Crie uma conta</p>
                <p className="p-subtitle p-white p-center">Insira seus dados para se cadastrar</p>
            </div>
            <TextInput
                type={"email"}
                value={email}
                setValue={setEmail}
                placeholder={"Insira seu email"}
                icoLeft={<Envelope size={18} color="var(--color-black)" />}
            />
            <TextInput
                type={isPassVisible ? "text" : "password"}
                value={password}
                setValue={setPassword}
                placeholder={"Insira sua senha"}
                icoLeft={<LockOn size={18} color="var(--color-black)" />}
                icoRight={<EyeOpen className={"button"} size={18} color="var(--color-black)" />}
            />
            <TextInput
                type={isPassVisibleC ? "text" : "password"}
                value={passwordC}
                setValue={setPasswordC}
                placeholder={"Repita a senha"}
                icoLeft={<LockOn size={18} color="var(--color-black)" />}
                icoRight={<EyeOpen className={"button"} size={18} color="var(--color-black)" />}
            />
            <TextInput
                type={isKeyVisible ? "text" : "password"}
                value={key}
                setValue={setKey}
                placeholder={"Código de confirmação"}
                icoLeft={<Key size={18} color="var(--color-black)" />}
                icoRight={<EyeOpen className={"button"} size={18} color="var(--color-black)" />}
            />
            <div className="login-form-gap-1">
                <TextBtn title={"Já possuo uma conta"} onClick={() => {
                    handleMove();
                    setCurrentPage(0);
                }} align={"right"} />
            </div>
            <RoundedBtn title={"Cadastrar-se"} hierarchy="primary" />
        </form>
    );
};

export default SignUp;
