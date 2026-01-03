import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateFormModal from "./components/CreateFormModal";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import BucketDetail from "./components/BucketDetail";
import type { Transaction } from "./types/Transaction";
import Pagination from "./components/Pagination";
import Filter from "./components/Filter";

function App() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);

  function UpdateTable(
    newamount: number,
    newdescription: string,
    newbucket: number,
    newcreated_at: string,
    isExpense: boolean
  ): void {
    setTransactions((prev) => [
      ...prev,
      {
        amount: newamount,
        description: newdescription,
        bucketId: newbucket,
        created_at: newcreated_at,
        isExpense: isExpense,
      },
    ]);
  }
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route
            path="/transactions"
            element={
              <>
                <Filter />
                <BucketList />
                <CreateFormModal updateTable={UpdateTable} />
                <TransactionTable transactions={transactions} />
                <Pagination setTransactions={setTransactions} />
              </>
            }
          />
          <Route path="transactions/:name" element={<BucketDetail />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
