import {type ChangeEvent, useState} from "react";
import {getErrorMessage} from "../utils/utils.ts";


type Row = {
    bucketId: number,
    description: string,
    amount: number,
    createdAt: string
}

const ImportForm = () => {

    const [file, setFile] = useState<string>("")
    const [errors, setErrors] = useState({uiMessage: ""})
    const [AlertisShown, setAlertisShown] = useState<boolean>(false)

    const errorStyle = {
        borderRadius: "5px",
        border: "1px solid #ced4da",
        boxShadow: errors["uiMessage"]
            ? "0 0 5px rgba(220, 53, 69, 0.5)"
            : "none",
        transition: "box-shadow 0.2s, border 0.2s"
    }

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        try {
            setAlertisShown(false)
            if (e.target.files) {
                const selectedFile = e.target.files[0]

                if (!selectedFile) return;

                const reader = new FileReader();

                reader.onload = (event) => {
                    const result = event.target?.result;
                    if (typeof result === "string") {
                        setFile(result)
                    }
                };

                reader.readAsText(selectedFile);
            }
        } catch (e) {
            console.error(e)
        }


    }

    const ReadData = (fileData: string) => {
        try {
            const lines = fileData.trim().split("\n");

            return lines.slice(1).map(line => {
                const values = line.split(";").map(value => value.trim());

                return {
                    bucketId: Number(values[0]),
                    description: values[1],
                    amount: Number(values[2]),
                    createdAt: values[3]
                };
            });

        } catch (e) {
            console.error(e)
            throw e
        }
        

    }

    const SubmitData = async () => {

        try {
            const transactions = ReadData(file)

            const response = await fetch("https://localhost:7118/api/v1/Transactions/import", {
                credentials: "include",
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(transactions)
            })
            if (!response.ok) {
                let message = "Something went wrong."

                if (response.status == 401) {
                    message = "Unauthorized access."
                }
                setErrors(prev => ({
                    ...prev,
                    uiMessage: message
                }))

                // implicit return to stop flow
                return;
            }
            const data = await response.json()

            console.log(data);
            setAlertisShown(true)
            return data;
        } catch (e) {
            const message = getErrorMessage(e);
            console.error(message);
            setErrors(prev => ({
                ...prev,
                uiMessage: "Not able to upload transactions of uploaded file."
            }));
        }

    }

    return (
        <div>
            <div style={errorStyle}>
                {errors["uiMessage"] && (
                    <p style={{color: "red", marginTop: "0.25rem"}}>{errors["uiMessage"]}</p>)}
            </div>

            {AlertisShown && <div className="alert alert-success" role="alert">
                Transactions are created successfully from file.
            </div>}
            
            
            <div className="mb-3">
                <label htmlFor="formFile" className="form-label">Upload file here</label>
                <input className="form-control" type="file" id="formFile" onChange={handleFileChange} accept=".csv"/>
            </div>
            {file && (
                <div className="card mb-3">
                    <div className="card-header">
                        Preview
                    </div>
                    <div className="table-responsive">
                        <table className="table table-sm mb-0">
                            <thead>
                            <tr>
                                <th>Bucket ID</th>
                                <th>Description</th>
                                <th>Amount</th>
                                <th>Date</th>
                            </tr>
                            </thead>
                            <tbody>
                            {ReadData(file).slice(0, 5).map((row: Row, idx: number) => (
                                <tr key={idx}>
                                    <td>{row.bucketId}</td>
                                    <td>{row.description}</td>
                                    <td>€{row.amount.toFixed(2)}</td>
                                    <td>{row.createdAt}</td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                    </div>
                    <div className="card-footer text-muted small">
                        {ReadData(file).length} rows {ReadData(file).length > 5 && `(showing first 5)`}
                    </div>
                </div>
            )}
            <button type="submit" className="btn btn-primary" onClick={SubmitData}>Upload file</button>
        </div>
    )
}

export default ImportForm;