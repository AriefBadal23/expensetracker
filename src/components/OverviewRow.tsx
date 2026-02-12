import type { Transaction } from "../types/Transaction";
import type { Bucket } from "../types/Bucket";


export type BucketTransactions = {
    bucketId: string,
    bucketName:string,
    transactions: Transaction[],
   
    
}

export type TransactionsSummary = {
    month: string,
    year: string,
    buckets: BucketTransactions[],
    totalIncome: number,
    totalExpenses: number
}

interface OverviewRowProps{
    data: TransactionsSummary
}
const OverviewRow = ({ data }:OverviewRowProps) => {
    if (!data || !data.buckets) return null;

    return data.buckets.flatMap(bucket => {
        
        const transactions = bucket.transactions;
        
        if (!transactions || transactions.length === 0) return null;

        return transactions.map((x:Transaction, index) => (
            <tr key={`${bucket.id}-${x.id}`}>
                {index === 0 && <td rowSpan={transactions.length}><strong>{bucket.bucketName}</strong></td>}
                <td>{x.description}</td>
                <td>{x.created_at}</td>
                <td>{x.isIncome ? "Income" : "Expense"}</td>
                <td>€{x.amount}</td>
            </tr>
        ));
    });
};


export default OverviewRow;