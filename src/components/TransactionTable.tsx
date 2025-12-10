import type { Transaction } from "../types/Transaction";
import TransactionRow from "./TransactionRow";

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
            <th scope="col">Date</th>
          </tr>
        </thead>
        <tbody>
          <TransactionRow transactions={transactions} />
        </tbody>
      </table>
    </>
  );
};
export default TransactionTable;
