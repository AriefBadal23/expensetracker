import {useEffect, useState} from "react";
import type {TransactionsSummary} from "../components/OverviewRow"
import OverviewRow from "./OverviewRow"
import TotalCard from "./TotalCard"
import {BucketType} from "../types/BucketType.tsx"
import {getErrorMessage} from "../utils/utils.ts";

const BucketOverviewTable = () => {
    // fetch doen naar:
    // http://localhost:5286/api/v1/buckets/summary?month=3&year=2027
    
    // useState()
    const [transactions, setTransactionsData] = useState<TransactionsSummary | undefined>(undefined);
    const [data, setData] = useState({
        month:"1",
        year: "2025"
    })
    const [errorMessage, setErrorMessage] = useState<Error | undefined>()
    const [isPending, setPending] = useState<boolean>(true)
    const ErrorMessageStyle = {
        color: "#B00020",
        backgroundColor: "#FFEBEE",
        borderLeft: "4px solid #D32F2F",
        padding: "8px 12px",
        borderRadius: "4px",
        fontSize: "16px",
        lineHeight: "1.4",
        fontFamily: "Segoe UI, Tahoma, sans-serif",
        marginTop: "6px"
    };
    // useEffect()
    useEffect(() => {
        const fetchSummary =  async () =>
        {
            try{
                const url = `http://localhost:5286/api/v1/buckets/summary?month=${data.month}&year=${data.year}`
                
                const response = await fetch(url)
                
                if (!response.ok) {
                    return;
                }
                const result = await response.json();

                if(typeof result.value !== 'object'){
                    throw new Error("Failed to fetch summary of bucket data")
                    
                }
                else{
                    setPending(false)
                    setTransactionsData(result.value)
                    
                }
            }
            catch(error) {
                const message = getErrorMessage(error)
                // show generic message in the UI for the user.
                setErrorMessage(new Error("Failed to fetch transactions data"))
                setPending(false)
                // log actual message
                console.log(message)
            }
        }
        
        fetchSummary()
    },
    [data])
    return (
        <>
        
            <div className="bucket-list">
                <TotalCard icon="💰" name="Income" type={BucketType.Income} data={transactions}/>
                <TotalCard icon="💸" name="Expenses" type={BucketType.Expense} data={transactions}/>
            </div>
                <div>
                    
                    {isPending &&  <div className="spinner-border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>}
                    {errorMessage && !isPending && <div className={'text-danger'}><p style={ErrorMessageStyle}>{errorMessage.message}</p></div>}

                    {
                        !errorMessage && !isPending &&
                        <>
                            <div>
                                {/*Predefined values*/}
                                <select name="months" onChange={(e) => {
                                    setData({...data, month: e.target.value})
                                }}>
                                    <option value="1">January</option>
                                    <option value="2">February</option>
                                    <option value="3">March</option>
                                    <option value="4">April</option>
                                    <option value="5">May</option>
                                    <option value="6">June</option>
                                    <option value="7">July</option>
                                    <option value="8">August</option>
                                    <option value="9">September</option>
                                    <option value="10">October</option>
                                    <option value="11">November</option>
                                    <option value="12">December</option>
                                </select>

                                <select name="year" onChange={(e) => {
                                    setData({...data, year: e.target.value})
                                }}>
                                    <option value="2025">2025</option>
                                    <option value="2026">2026</option>
                                </select>
                            </div>
                        <table className="table">
                            <thead>
                            <tr>
                                <th scope="col">Bucket</th>
                                <th scope="col">Transaction</th>
                                <th scope="col">Date</th>
                                <th scope="col">Type</th>
                                <th scope="col">Amount</th>
                            </tr>
                            </thead>
                            <tbody>
                            <OverviewRow data={transactions}/>
                            </tbody>
                        </table>
                        </>
                    }

                </div>
        </>
    )
}

export default BucketOverviewTable;