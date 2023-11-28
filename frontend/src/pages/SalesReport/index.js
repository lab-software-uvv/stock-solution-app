import React, { useState } from "react";
import Requests from "../../services/requests";
import toast from "react-hot-toast";

function ReportScreen() {
  const [formData, setFormData] = useState({
    initialDate: "",
    endDate: "",
  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((prevState) => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
  
    try {
      const response = await Requests.get("/reports/vendas-report", {
        params: {
          InitialSaleDate: formData.initialDate,
          FinalSaleDate: formData.endDate,
        },
        responseType: "blob",
      })
      .then(response => {
        console.log(response);

        // Create a Blob from the response data
        const blob = new Blob([response.data], { type: 'text/csv' });

        // Create a temporary link element and trigger the download
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'Vendas.csv'; // Use the suggested filename
        document.body.appendChild(link);
        link.click();

        // Clean up
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    })
    } catch (error) {
      console.error("Error downloading CSV:", error);
      toast.error("Erro ao baixar o arquivo CSV");
    }
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <label>
        Data de Início:
        <input
          type="date"
          name="initialDate"
          value={formData.initialDate}
          onChange={handleChange}
        />
      </label>
      <label>
        Data de Fim:
        <input
          type="date"
          name="endDate"
          value={formData.endDate}
          onChange={handleChange}
        />
      </label>
      <input type="submit" value="Submit" />
    </form>
  );
}

export default ReportScreen;
