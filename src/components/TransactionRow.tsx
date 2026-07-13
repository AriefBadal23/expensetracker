import type {Dispatch, SetStateAction} from "react";
import type {Transaction} from "../types/Transaction";
import {Buckets} from "../types/Buckets.tsx";
import type {Bucket} from "../types/Bucket.tsx";


interface TransactionRowProps {
  transaction: Transaction;
  setTransactions: Dispatch<SetStateAction<Transaction[]>>
    setShowModal: Dispatch<SetStateAction<boolean>>
    setUpdateForm:Dispatch<SetStateAction<boolean>>
    // seperate state for the transaction to track changes (see TransactionTable)
    setUpdateTransaction:Dispatch<SetStateAction<Transaction>>
    buckets: Bucket[]
}

const TransactionRow = ({transaction, setTransactions, buckets, setShowModal, setUpdateForm, setUpdateTransaction }: TransactionRowProps) => {
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
          <td>{buckets.find(bucket => bucket.bucket.id === transaction.bucketId)?.bucket.name === Buckets.Salary ? `   + €${transaction.amount}` : ` - € ${transaction.amount}`}</td>
          <td>{buckets.find(bucket => bucket.bucket.id === transaction.bucketId)?.bucket.name}</td>
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
