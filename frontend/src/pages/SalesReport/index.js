import React, { useState } from "react";
import "./styles.css";
import Requests from "../../services/requests";
import toast from "react-hot-toast";
import Navigator from "../../components/scenes/navigator";
import TextInput from "../../components/ui/text.input";
import IconBtn from "../../components/ui/icon.btn";
import { ArrowBackThickFill, Download } from "akar-icons";
import ShrinkBtn from "../../components/ui/shrink.btn";
import { useNavigate } from "react-router-dom";

function ReportScreen({ user, setAuth }) {
    const navigate = useNavigate();

    const [initialDate, setInitialDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());

    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await Requests.get("/reports/vendas-report", {
                params: {
                    InitialSaleDate: initialDate,
                    FinalSaleDate: endDate,
                },
                responseType: "blob",
            }).then((response) => {
                console.log(response);

                // Create a Blob from the response data
                const blob = new Blob([response.data], { type: "text/csv" });

                // Create a temporary link element and trigger the download
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement("a");
                link.href = url;
                link.download = "Vendas.csv"; // Use the suggested filename
                document.body.appendChild(link);
                link.click();

                // Clean up
                document.body.removeChild(link);
                window.URL.revokeObjectURL(url);
            });
        } catch (error) {
            console.error("Error downloading CSV:", error);
            toast.error("Erro ao baixar o arquivo CSV");
        }
    };

    const Form = () => {
        return (
            <form onSubmit={handleSubmit} className="gap-10 flex-column">
                <div>
                    <p className="p-text">Data de início *</p>
                    <TextInput
                        required={true}
                        type="date"
                        name="initialDate"
                        value={initialDate}
                        setValue={setInitialDate}
                    />
                </div>
                <div>
                    <p className="p-text">Data de fim *</p>
                    <TextInput
                        required={true}
                        type="date"
                        name="endDate"
                        value={endDate}
                        setValue={setEndDate}
                    />
                </div>
                <div style={{ marginTop: 20 }}></div>
                <ShrinkBtn
                    action={(e) => {
                        handleSubmit(e);
                    }}
                    text={"Baixar arquivo"}
                    backgroundColor={"var(--color-primary)"}
                    mouseOnBg={"var(--color-primary)"}
                    shrink={true}
                    width={250}
                >
                    <Download color="white" />
                </ShrinkBtn>
                {/* <input type="submit" value="Submit" /> */}
            </form>
        );
    };

    return (
        <Navigator user={user} setAuth={setAuth}>
            <div className="report-wrapper flex-center flex-column">
                <div className="report-form flex-column flex-center gap-30">
                    <div className="flex-row gap-20">
                        <ArrowBackThickFill
                            className="button"
                            color="var(--color-darkgrey)"
                            onClick={() => navigate("/dashboard")}
                        />
                        <p className="p-grey">Gerar Relarório de vendas</p>
                    </div>
                    <Form />
                </div>
            </div>
        </Navigator>
    );
}

export default ReportScreen;
