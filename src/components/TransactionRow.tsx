import type {Dispatch, SetStateAction} from "react";
import type {Transaction} from "../types/Transaction";
import {IdToBucket} from "../utils/BucketMap";
import {Buckets} from "../types/Buckets.tsx";


interface TransactionRowProps {
  transaction: Transaction;
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
    setShowModal: Dispatch<SetStateAction<boolean>>
    setUpdateForm:Dispatch<SetStateAction<boolean>>
    setUpdateTransaction:Dispatch<SetStateAction<Transaction>>
}

const TransactionRow = ({transaction, setTransactions, setShowModal, setUpdateForm, setUpdateTransaction }: TransactionRowProps) => {
  async function DeleteTransaction(id: number | undefined) {
    try {
          await fetch(`https://localhost:7118/api/v1/transactions/${id}`, {
          method: "Delete", 
              credentials: "include",
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
      <tr key={transaction.id} id={transaction.id?.toString()}>
          <td>{transaction.description}</td>
          <td>{IdToBucket[transaction.bucketId] === Buckets.Salary ? `   + €${transaction.amount}` : ` - € ${transaction.amount}`}</td>
          <td>{IdToBucket[transaction.bucketId]}</td>
          <td>{new Date(transaction.createdAt).toLocaleDateString()}</td>
          <td>
              <button type="button"
                      aria-label="Delete transaction"
                      onClick={() => DeleteTransaction(transaction.id)}>
                  <img src="delete.png" alt="Delete transaction"/></button>
          </td>

          <td>
              <button type="button"
                      aria-label="Update transaction"
                      onClick={() => {
                          setShowModal(true)
                          setUpdateForm(true)
                          setUpdateTransaction(transaction)
                      }}>
                  <img src="update.png" alt="Update transaction"/></button>
          </td>
      </tr>
  );
};

export default TransactionRow;
