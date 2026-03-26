import type {Transaction} from "../types/Transaction";
import {BucketType} from "../types/BucketType.tsx";


export type BucketTransactions = {
    bucketId: string,
    bucketName:string,
    transactions: Transaction[],
    bucketType: BucketType
   
    
}

export type TransactionsSummary = {
    month: string,
    year: string,
    buckets: BucketTransactions[],
    totalIncome: number,
    totalExpenses: number
}

interface OverviewRowProps{
    data: TransactionsSummary | undefined
}
const OverviewRow = ({ data }:OverviewRowProps) => {
    if (!data || !data.buckets) return null;

    return data.buckets.flatMap(bucket => {
        if(bucket !== null){
            
            const transactions = bucket.transactions;
            
            if (!transactions || transactions.length === 0) return null;
    
            return transactions.map((x:Transaction, index) => (
                <tr key={`${bucket.bucketId}-${x.id}`}>
                    {index === 0 && <td rowSpan={transactions.length}><strong>{bucket.bucketName}</strong></td>}
                    <td>{x.description}</td>
                    <td>{new Date(x.created_at).toLocaleDateString()}</td>
                    <td>{bucket.bucketType}</td>
                    <td>€{x.amount}</td>
                </tr>
            ));
            
        }
    });
};


export default OverviewRow;