import {type Dispatch, type SetStateAction} from "react";
import type {Transaction} from "../types/Transaction";
import TransactionRow from "./TransactionRow";
import {useState} from "react";
import CreateFormModal from "./CreateFormModal.tsx";
import type {Bucket} from "../types/Bucket.tsx";
import BucketTable from "./BucketTable.tsx";

interface TransactionTableProps {
    buckets: Bucket[],
    transactions: Transaction[],
    setTransactions: Dispatch<SetStateAction<Transaction[]>>,
    ErrorMessage: Error | undefined,
    setBuckets: Dispatch<SetStateAction<Bucket[]>>
}

const TransactionTable = ({
                              buckets,
                              transactions,
                              setTransactions,
                              ErrorMessage,
                              setBuckets
                          }: TransactionTableProps) => {
    const [showModal, setShowModal] = useState(false);

    const [showBucketModal, setShowBucketModal] = useState(false);

    // state to show the CreateFormTransactionForm with the transaction details.
    const [isUpdateForm, setUpdateForm] = useState(false)

    const [SelectedTransactions, setSelectedTransactions] = useState<number[]>([])
    
    
    // keep track of the transaction that need to be updated, with a separate state, lift state up to this component instead of in the transaction row.
    const [updatedTransaction, setUpdatedTransaction] = useState<Transaction>({
        id: 0,
        createdAt: new Date(),
        bucketId: 0,
        description: "",
        amount: 0
    });

    const ErrorMessageStyle = {
        color: "#B00020",
        backgroundColor: "#FFEBEE",
        borderLeft: "4px solid #D32F2F",
        padding: "8px 12px",
        borderRadius: "4px",
        fontSize: "16px",
        lineHeight: "1.4",
        fontFamily: "Segoe UI, Tahoma, sans-serif",
        marginTop: "6px"
    };

    const selectAllTransactions = () => {
        // loop over alle transacties en kijk of de vorige array de transaction.id bevat zo niet dan voeg ik het aan de array toe.
        transactions.forEach(t => setSelectedTransactions(prev => prev.includes(t.id) ? [...prev] : [...prev, t.id]))
    }

    const DeleteSelectedTransactions = async () => {
        if (SelectedTransactions.length === 0) {
            alert("Please select transactions to delete");
            return;
        }


        if (!confirm(`Are you sure you want to delete ${SelectedTransactions.length} transaction(s)?`)) {
            return;
        }

        try {
            const response = await fetch("https://localhost:7118/api/v1/Transactions/bulk", {
                method: "DELETE",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(SelectedTransactions)
            });

            if (response.ok) {
                setTransactions((prev) =>
                    prev.filter(t => !SelectedTransactions.includes(t.id || 0))
                );
                setSelectedTransactions([]);
                alert("Transactions deleted successfully");
            } else {
                alert("Failed to delete transactions");
            }
        } catch (err) {
            console.error(err);
            alert("Error deleting transactions");
        }
    };
    return (
        <>

            {/*Show create modal*/}
            {showModal ?
                <CreateFormModal buckets={buckets} SetShowModal={setShowModal} transactionID={updatedTransaction?.id}
                                 showModal={showModal} isUpdateForm={isUpdateForm} setUpdateForm={setUpdateForm}
                                 setTransactions={setTransactions}/>
                : null
            }

            {/*Show error message if any*/}
            {ErrorMessage &&
                <div className={'text-danger'}><p style={ErrorMessageStyle}>{ErrorMessage.message}</p></div>}

            {/*If no error message and data is fetched show table.*/}
            {!ErrorMessage &&
                <div className="container mt-4">
                <span>
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={() => {
                            setShowModal(true)

                        }}
                    >
                    Add new transaction
                </button>
                </span>
                    <span style={{color: 'blue', padding: 15}}>
                    <a href="/overview">
                        <button
                            type="button"
                            className="btn btn-info"
                        >
                            Overview by month
                        </button>
                    </a>
                </span>
                    <span style={{padding: 5}}>
                    <button
                        type="button"
                        className="btn btn-info"
                        onClick={() => setShowBucketModal(true)}
                    >
                        Manage buckets
                    </button>
                </span>
                    <span style={{padding: 5}}>
                    <button
                        type="button"
                        className="btn btn-danger"
                        onClick={DeleteSelectedTransactions}
                        disabled={SelectedTransactions.length === 0}
                    >
                        Delete Selected ({SelectedTransactions.length})
                    </button>
                </span>
                    <span style={{padding: 5}}>
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={selectAllTransactions}
                    >
                        Select all ({transactions.length})
                    </button>
                </span>
                    <table className="table">
                        <thead>
                        <tr>
                            <th scope="col"></th>
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
                            showBucketModal ? <BucketTable setShowBucketModal={setShowBucketModal} setBuckets={setBuckets} buckets={buckets}/> :
                                transactions.map((t) => {
                                    return (
                                        <tr key={t.id}>
                                            <td>
                                                <input
                                                    type="checkbox"
                                                    checked={SelectedTransactions.includes(t.id || 0)}
                                                    onChange={() =>
                                                        setSelectedTransactions((prev) =>
                                                            prev.includes(t.id || 0)
                                                                ? prev.filter(id => id !== t.id)
                                                                : [...prev, t.id || 0]
                                                        )
                                                    }
                                                />
                                            </td>
                                            <TransactionRow transaction={t} setTransactions={setTransactions}
                                                            buckets={buckets} setShowModal={setShowModal}
                                                            setUpdateForm={setUpdateForm}
                                                            setUpdateTransaction={setUpdatedTransaction}/>
                                        </tr>
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
