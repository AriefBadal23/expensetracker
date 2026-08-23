import {useEffect, useState} from "react";
import type {TransactionsSummary} from "./OverviewRow.tsx"
import OverviewRow from "./OverviewRow"
import TotalCard from "./TotalCard"
import {BucketType} from "../types/BucketType.tsx"
import {getErrorMessage} from "../utils/utils.ts";

const BucketOverviewTable = () => {
    // fetch doen naar:
    // http://localhost:5286/api/v1/buckets/summary?month=3&year=2027
    
    // useState()
    const [transactions, setTransactionsData] = useState<TransactionsSummary | undefined>(undefined);
    const currentYear = new Date().getFullYear()
    const currentMonth = new Date(Date.now()).getMonth() + 1;


    const [data, setData] = useState<{ month: string, year: string }>({
        month: currentMonth.toString(),
        year: currentYear.toString()
    })
    const months: string[] = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    
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
                const url = `https://localhost:7118/api/v1/buckets/summary?month=${data.month}&year=${data.year}`
                
                const response = await fetch(url, {
                    credentials: "include"
                })
                
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
                console.error(message)
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
                                }} id="selectedMonth">
                                    {
                                        // cleaner way to show the months as an option element
                                        months.map((month) => <option key={month} value={month}>{month}</option>
                                        )
                                    }
                                </select>

                                <select name="year" onChange={(e) => {
                                    setData({...data, year: e.target.value})
                                }}>
                                    <option value="2025">2025</option>
                                    {/*// current year selected*/}
                                    <option value="2026" selected>2026</option>
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