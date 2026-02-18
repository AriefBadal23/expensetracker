import { type Dispatch, type SetStateAction } from "react";
import type { Transaction } from "../types/Transaction";
import TransactionRow from "./TransactionRow";
import {useState} from "react";
import CreateFormModal from "./CreateFormModal.tsx";

interface TransactionTableProps {
  transactions: Transaction[];
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
}
const TransactionTable = ({ transactions, setTransactions }: TransactionTableProps) => {
  const [showModal, setShowModal] = useState(false);
      
  return (
    <>
      <button
          type="button"
          className="btn btn-primary"
          onClick={() => {
            setShowModal(true)

          }}
      >
        Add new transaction
      </button>

      {
          
        showModal ? 
            <CreateFormModal SetShowModal={setShowModal} showModal={showModal} isUpdateForm={false}/>
            : 
            <>
            </>
      }
      <table className="table">
        <thead>
          <tr>
            <th scope="col">Transaction</th>
            <th scope="col">Amount</th>
            <th scope="col">Bucket</th>
            <th scope="col">Date</th>
            <th scope="col">Delete</th>
            <th scope="col">Update</th>
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
