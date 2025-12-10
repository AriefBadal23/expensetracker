import type { Transaction } from "../types/Transaction";
import { IdToBucket } from "../utils/BucketMap";

interface TransactionRowProps {
  transactions: Transaction[];
}

const TransactionRow = ({ transactions }: TransactionRowProps) => {
  return (
    <>
      {transactions.map((t: Transaction) => {
        return (
          <tr key={t.id}>
            <td>{t.description}</td>
            <td>{t.amount}</td>
            <td>{IdToBucket[t.bucketId]}</td>
            <td>{new Date(t.created_at).toLocaleDateString()}</td>
          </tr>
        );
      })}
    </>
  );
};

export default TransactionRow;
