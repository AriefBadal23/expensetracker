import { useState } from "react";
import "./App.css";
import BucketList from "./components/BucketList";
import TransactionTable from "./components/TransactionTable";
import CreateFormModal from "./components/CreateFormModal";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import type { Transaction } from "./types/Transaction";
import Pagination from "./components/Pagination";
import Filter from "./components/Filter";
import Overview from "./components/Overview"
import LoginForm from "../LoginForm.tsx";

function App() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [errorMessage, setErrorMessage] = useState<Error | undefined>();
  
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/login"
          element={<LoginForm/>}
          />
          <Route
          path="/overview"
          element={<Overview/>}
          />
          <Route
            path="/"
            element={
              <>
                <BucketList transactions={transactions} />
                <CreateFormModal isUpdateForm={false} setTransactions={setTransactions}  />
                <Filter />
                <TransactionTable transactions={transactions} setTransactions={setTransactions} ErrorMessage={errorMessage} />
                <Pagination  setTransactions={setTransactions} setErrorMessage={setErrorMessage}/>
              </>
            }
          />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
