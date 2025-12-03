import TransactionRow from "./TransactionRow";
import type { Transaction } from "../types/Transaction";
import { v4 as uuidv4 } from "uuid";

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
                key={uuidv4()}
                description={t.description}
                amount={t.amount}
                bucket={t.bucket.toString()}
              />
            );
          })}
        </tbody>
      </table>
    </>
  );
};
export default TransactionTable;
