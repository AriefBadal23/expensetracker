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

  function SortOnCreation(){
    
  }
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
            <>
              <tr key={t.id}>
                <td>{t.description}</td>
                {/* TODO: dit kan beter: */}
                <td>€ {t.isIncome ? ` + ${t.amount}` : `- ${t.amount}`}</td>
                <td>{IdToBucket[t.bucketId]}</td>
                <td>{new Date(t.created_at).toLocaleDateString()}</td>
                <td onClick={() => DeleteTransaction(t.id)}><a><img src="delete.png"/></a></td>
                <td key={t.id}><img src="update.png" onClick={() => {
                  setShowModal(true)
                  setSelectedTransaction(t)
                }}/></td>
              </tr>
              
            </>
        );
      })}
      
    </>
  );
};

export default TransactionRow;
