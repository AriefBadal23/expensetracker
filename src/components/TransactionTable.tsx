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
          </tr>
        </thead>
        <tbody>
          {transactions.map((t) => {
            return (
              <TransactionRow description={t.description} amount={t.amount} />
            );
          })}
        </tbody>
      </table>
    </>
  );
};
export default TransactionTable;
