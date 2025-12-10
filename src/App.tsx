import { useEffect, useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateFormModal from "./components/CreateFormModal";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import BucketDetail from "./components/BucketDetail";
import type { Transaction } from "./types/Transaction";

function App() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);

  useEffect(() => {
    const fetchTransactions = async () => {
      try {
        const response = await fetch(
          "http://localhost:5286/api/v1/transactions"
        );
        const transactions = await response.json();
        setTransactions(transactions);
      } catch {
        console.log("Failed to fetch data from api");
      }
    };
    fetchTransactions();
  }, []);

  function UpdateTable(
    newamount: number,
    newdescription: string,
    newbucket: number
  ): void {
    setTransactions((prev) => [
      ...prev,
      {
        amount: newamount,
        description: newdescription,
        bucketId: newbucket,
      },
    ]);
  }
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route
            path="/"
            element={
              <>
                <BucketList />
                <CreateFormModal updateTable={UpdateTable} />
                <TransactionTable transactions={transactions} />
              </>
            }
          />
          <Route path=":name" element={<BucketDetail />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
