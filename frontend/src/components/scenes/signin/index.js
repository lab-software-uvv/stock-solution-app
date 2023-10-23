import React, { useState } from "react";
import "./styles.css";

//components
import TextInput from "../../ui/text.input";
import TextBtn from "../../ui/text.btn";
import RoundedBtn from "../../ui/rounded.btn";

//assets
import { Envelope, LockOn, EyeOpen } from "akar-icons";

const SignIn = ({ setCurrentPage, handleMove, setAuth }) => {
    const [isPassVisible, setIsPassVisible] = useState(false);

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleSignIn = () => {
        setAuth({ auth: true, token: null });
    };

    return (
        <form className="login-form">
            <div className="login-form-gap-2">
                <p className="p-title p-white p-center">Bem-vindo!</p>
                <p className="p-subtitle p-white p-center">Insira seus dados para se conectar</p>
            </div>
            <TextInput
                type={"email"}
                value={email}
                setValue={setEmail}
                placeholder={"Email"}
                icoLeft={<Envelope size={18} color="var(--color-black)" />}
            />
            <TextInput
                type={isPassVisible ? "text" : "password"}
                value={password}
                setValue={setPassword}
                placeholder={"Senha"}
                icoLeft={<LockOn size={18} color="var(--color-black)" />}
                icoRight={<EyeOpen size={18} color="var(--color-black)" />}
            />
            <div className="login-form-gap-1">
                <TextBtn title={"Esqueci minha senha"} align={"right"} />
            </div>
            <RoundedBtn title={"Entrar"} onClick={handleSignIn} hierarchy="primary" />
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
