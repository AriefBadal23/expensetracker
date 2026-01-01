import { useEffect, useState } from "react";
import type { Transaction } from "../types/Transaction";
import { useSearchParams } from "react-router-dom";

interface PaginationProps {
  setTransactions: (transactions: Transaction[]) => void;
}

const Pagination = ({ setTransactions }: PaginationProps) => {
  const [page, SetPage] = useState(1);
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [error, setError] = useState<string>();
  const [total, setTotal] = useState<number>(1);

  const [search] = useSearchParams();

  const PAGESIZE = 10;
  const TOTALPAGES = total / PAGESIZE;

  useEffect(() => {
    const fetchTransactions = async () => {
      try {
        const response = await fetch(
          `http://localhost:5286/api/v1/transactions?pageNumber=${page}&pageSize=${PAGESIZE}&bucket=${search.get(
            "id"
          )}`
        );

        const data = await response.json();
        setTotal(data["total"]);
        setTransactions(data["transactions"]);
      } catch (error) {
        if (error instanceof Error) {
          setError(error.message);
        } else {
          setError("Something went wrong");
          console.log(error);
        }
      }
    };
    fetchTransactions();
  }, [page, setTransactions, search]); //! beide als dependancy

  return (
    <div>
      <p>
        Page: {page}/{Math.ceil(TOTALPAGES)}
      </p>
      <input
        type="button"
        value="Prev"
        disabled={page === 1}
        onClick={() => {
          if (page > 0) {
            SetPage(page - 1);
          }
        }}
      />
      <input
        type="button"
        value="Next"
        disabled={page === Math.ceil(TOTALPAGES)} // why does it work only with ===
        onClick={() => {
          if (page <= TOTALPAGES) {
            SetPage(page + 1);
          }
        }}
      />
    </div>
  );
};

export default Pagination;
