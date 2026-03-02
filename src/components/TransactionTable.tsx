import { type Dispatch, type SetStateAction } from "react";
import type { Transaction } from "../types/Transaction";
import TransactionRow from "./TransactionRow";
import {useState} from "react";
import CreateFormModal from "./CreateFormModal.tsx";

interface TransactionTableProps {
    transactions: Transaction[];
    setTransactions: Dispatch<SetStateAction<Transaction[]>>
    ErrorMessage: Error|undefined
}
const TransactionTable = ({ transactions, setTransactions,ErrorMessage }: TransactionTableProps) => {
    const [showModal, setShowModal] = useState(false);
    return (
    <>
        
        {/*Show create modal*/}
        { showModal ? 
            <CreateFormModal SetShowModal={setShowModal} showModal={showModal} isUpdateForm={false} setTransactions={setTransactions}/>
            : null
        }

        {/*Show error message if any*/}
        {ErrorMessage && <div>{ErrorMessage.message}</div>}
        
        {/*If no error message and data is fetched show table.*/}
        {!ErrorMessage && transactions.length > 0  &&
            <div>
                <button
                    type="button"
                    className="btn btn-primary"
                    onClick={() => {    
                        setShowModal(true)

                    }}
                >
                    Add new transaction
                </button>
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
                    {
                        transactions.map((t) => {
                            return (
                                <TransactionRow key={t.id} transaction={t} setTransactions={setTransactions} /> 
                            )
                        })
                    }
                    </tbody>
                </table>
            </div>
        
        }
    </>
  );
};
export default TransactionTable;
