import "../styles/Bucket.css";
import type {TransactionsSummary} from "./OverviewRow.tsx"
import {TransactionType} from "../types/TransactionType"

interface ITotalCard{
    name:string,
    type: TransactionType,
    icon: string,
    data?: TransactionsSummary
}
const TotalCard = ({name, type, icon, data}:ITotalCard) => {
    let amount: number;

    if (type === TransactionType.Income) {
        amount = data ? data.totalIncome : 0;
    } else {
        amount = data ? data.totalExpenses : 0;
    }


    return (
        <div className="bucket-card">
            <p id="icon">{icon}</p>
            <p id="name">{name}</p>
            <p id="amount">Total: €{amount}</p>
        </div>
        
    )
}
export default TotalCard