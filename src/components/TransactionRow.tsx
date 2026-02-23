import type { Dispatch, SetStateAction } from "react";
import type { Transaction } from "../types/Transaction";
import { IdToBucket } from "../utils/BucketMap";
import CreateFormModal from "./CreateFormModal.tsx";
import {useState, useEffect} from "react"
interface TransactionRowProps {
  transactions: Transaction[];
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
}

const TransactionRow = ({ transactions, setTransactions }: TransactionRowProps) => {
  const [showModal, setShowModal] = useState(false);
  const [selectedTransaction, setSelectedTransaction] = useState<Transaction | null>(null);

  useEffect(() => {
    
  }, [selectedTransaction]);
  
  function DeleteTransaction(id: number | undefined) {
    try {
      fetch(`http://localhost:5286/api/v1/transactions/${id}`, {
        method: "Delete",
  
        headers: {
          "Content-type": "application/json; charset=UTF-8"
        },
      })
      // trigger a re-render and show the new array.
      setTransactions((prev) => prev.filter(t => t.id !== id) )

    }
    catch (err) {
      console.log(err)
    }
    
  }
  
  return (
      // fragments
    <>
        {
          showModal && selectedTransaction !== null ?
              <CreateFormModal SetShowModal={setShowModal} showModal={showModal} isUpdateForm={true} transactionID={selectedTransaction.id} />
              : <></>
        }
      
      {transactions.map((t: Transaction) => {
        return (
              <tr >
                <td>{t.description}</td>
                {/* TODO: dit kan beter: */}
                <td>€ {t.isIncome ? ` + ${t.amount}` : `- ${t.amount}`}</td>
                <td>{IdToBucket[t.bucketId]}</td>
                <td>{new Date(t.created_at).toLocaleDateString()}</td>
                <td><button type="button" 
                      aria-label="Delete transaction"
                      onClick={() => DeleteTransaction(t.id)}>
                     <img src="delete.png" alt="Delete transaction"/></button></td>
                
                <td><button type="button"
                            aria-label="Update transaction"
                            onClick={() => {
                            setShowModal(true)
                            setSelectedTransaction(t)}}>
                <img src="update.png" alt="Update transaction" /></button></td>
              </tr>
              
        );
      })}
      
    </>
  );
};

export default TransactionRow;
