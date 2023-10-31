const regex = {
    email: /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g,
    username: /^[A-Za-z0-9_.]{5,19}$/,
    password: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z$*&@#._)(&%!-]{6,}$/,
    cpf: /^[0-9]{11}/
};

export default regex;
