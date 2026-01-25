import { type Dispatch, type SetStateAction } from "react";
import type { Transaction } from "../types/Transaction";
import TransactionRow from "./TransactionRow";

interface TransactionTableProps {
  transactions: Transaction[];
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
}
const TransactionTable = ({ transactions, setTransactions }: TransactionTableProps) => {
  
  return (
    <>
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Transaction</th>
            <th scope="col">Amount</th>
            <th scope="col">Bucket</th>
            <th scope="col">Date</th>
            <th scope="col">Delete</th>
          </tr>
        </thead>
        <tbody>
          <TransactionRow transactions={transactions} setTransactions={setTransactions} />
        </tbody>
      </table>
    </>
  );
};
export default TransactionTable;
