import TransactionRow from "./TransactionRow";
import type { Transaction } from "../types/Transaction";

interface TransactionTableProps {
  transactions: Transaction[];
}
const TransactionTable = ({ transactions }: TransactionTableProps) => {
  return (
    <>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Transaction</th>
            <th scope="col">Amount</th>
            <th scope="col">Bucket</th>
          </tr>
        </thead>
        <tbody>
          {transactions.map((t) => {
            return (
              <TransactionRow
                description={t.description}
                amount={t.amount}
                bucket={t.bucket}
              />
            );
          })}
        </tbody>
      </table>
    </>
  );
};
export default TransactionTable;
