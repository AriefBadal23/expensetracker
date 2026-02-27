import type {Dispatch, SetStateAction} from "react";
import {useEffect, useState} from "react"
import type {Transaction} from "../types/Transaction";
import {IdToBucket} from "../utils/BucketMap";
import CreateFormModal from "./CreateFormModal.tsx";
import {Buckets} from "../types/Buckets.tsx";


interface TransactionRowProps {
  transaction: Transaction;
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
}

const TransactionRow = ({transaction, setTransactions }: TransactionRowProps) => {
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
              <CreateFormModal SetShowModal={setShowModal} showModal={showModal} isUpdateForm={true} transactionID={selectedTransaction.id} setTransactions={setTransactions} />
              : <></>
        }
        
              <tr key={transaction.id} id={transaction.id?.toString()}>
                <td>{transaction.description}</td>
                {/* TODO: dit kan beter: */}
                <td>{IdToBucket[transaction.bucketId] === Buckets.Salary ? `   + €${transaction.amount}` : ` - € ${transaction.amount}`}</td>
                <td>{IdToBucket[transaction.bucketId]}</td>
                <td>{new Date(transaction.created_at).toLocaleDateString()}</td>
                <td><button type="button" 
                      aria-label="Delete transaction"
                      onClick={() => DeleteTransaction(transaction.id)}>
                     <img src="delete.png" alt="Delete transaction"/></button></td>
                
                <td><button type="button"
                            aria-label="Update transaction"
                            onClick={() => {
                            setShowModal(true)
                            setSelectedTransaction(transaction)}}>
                <img src="update.png" alt="Update transaction" /></button></td>
              </tr>
    </>
  );
};

export default TransactionRow;
