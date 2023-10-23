import React, { useState } from "react";
import "./styles.css";

//components
import Navigator from "../../components/scenes/navigator";
import CrudContainer from "../../components/scenes/crudcontainer";
import TextInput from "../../components/ui/text.input";
import IconBtn from "../../components/ui/icon.btn";

import { DataGrid } from "@mui/x-data-grid";

//assets
import { Save, ShippingBoxV1 } from "akar-icons";
import { TextField } from "@mui/material";

//settings
const columns = [
    { field: "id", headerName: "id", width: 25 },
    { field: "name", headerName: "Nome", width: 125 },
    { field: "cod", headerName: "Código", width: 100 },
    { field: "cat", headerName: "Categoria", width: 100 },
    { field: "qtdd", headerName: "Quantidade", width: 100 },
    { field: "marca", headerName: "Marca", width: 100 },
    { field: "price", headerName: "Preço unitário", width: 100 },
    { field: "date1", headerName: "Aquisição", width: 100 },
    { field: "date2", headerName: "Vencimento", width: 100 },
];

const Products = ({ user }) => {
    const [productList, setProductList] = useState([]);
    const categories = ["Vegetais", "Bebidas", "Carnes", "Grãos"];

    const List = () => {
            return <div style={{ height: '50vh', width: '60vw' }}>
            <DataGrid rows={productList} columns={columns} />
          </div>;
        // return <></>;
    };

    const Form = () => {
        return (
            <form className="products-form">
                <div>
                    <p className="p-text">Nome do produto</p>
                    <TextInput placeholder={"Nome do produto"}></TextInput>
                </div>
                <div>
                    <p className="p-text">Código do produto</p>
                    <TextInput placeholder={"Código do produto"}></TextInput>
                </div>
                <div>
                    <p className="p-text">Quantidade adquirida</p>
                    <TextInput type="number" min="1" placeholder={"0"}></TextInput>
                </div>
                <div>
                    <p className="p-text">Marca</p>
                    <TextInput placeholder={"Nome do fabricante"}></TextInput>
                </div>
                <div>
                    <p className="p-text">Preço unitário</p>
                    <TextInput
                        type="number"
                        min="1"
                        step="any"
                        icoLeft={<p className="p-text p-price">R$</p>}
                        placeholder={"0,00"}
                    ></TextInput>
                </div>
                <div>
                    <p className="p-text">Categoria</p>
                    <select>
                        {categories.map((element) => {
                            return (
                                <option key={element} value={element}>
                                    {element}
                                </option>
                            );
                        })}
                    </select>
                </div>
                <div>
                    <p className="p-text">Data da aquisição</p>
                    <TextInput type="date" placeholder={"0,00"}></TextInput>
                </div>
                <div>
                    <p className="p-text">Vencimento</p>
                    <TextInput type="date" placeholder={"0,00"}></TextInput>
                </div>
                <div>
                    <p className="p-text">Descrição</p>
                    <TextInput type="textarea" placeholder={"Descrição do produto"}></TextInput>
                    
                </div>
                <div></div>
                <div></div>
                <div className="products-form-submit-wrapper">
                    <IconBtn backgroundColor={"var(--color-primary)"}>
                        <Save color="white" />
                    </IconBtn>
                </div>
            </form>
        );
    };

    return (
        <Navigator user={user}>
            <div className="products-wrapper flex-center">
                <CrudContainer
                    icon={<ShippingBoxV1 color="var(--color-darkgrey)" />}
                    title={"Produtos"}
                    list={<List />}
                    form={<Form />}
                ></CrudContainer>
            </div>
        </Navigator>
    );
};

export default Products;
