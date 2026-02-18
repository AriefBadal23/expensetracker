import { useEffect, useState } from "react";
import type { Transaction } from "../types/Transaction";
import { useSearchParams } from "react-router-dom";
import "react-day-picker/dist/style.css";

interface PaginationProps {
  setTransactions: (transactions: Transaction[]) => void;
}

const Pagination = ({ setTransactions }: PaginationProps) => {
  const [page, SetPage] = useState(1);
  const [error, setError] = useState<string>();
  const [total, setTotal] = useState<number>(1);

  const [search] = useSearchParams();

  const PAGESIZE = 10;
  const TOTALPAGES = total / PAGESIZE;

  useEffect(() => {
    const fetchTransactions = async () => {
      try {
          const month = search.get("month")
          const year = search.get("year") 
          const bucketId = search.get("id") 
          
          let url = `http://localhost:5286/api/v1/transactions?pageNumber=${page}&pageSize=${PAGESIZE}`
          
          if(year !== null ){
              url = url + `&year=${year}`
          }
          if(month !== null ){
              url = url + `&month=${month}`
          }
          if(bucketId !== null){
              url = url + `&bucket=${bucketId}`
          }
          
          const response = await fetch(url);

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
