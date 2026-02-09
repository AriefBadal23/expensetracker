import { useState } from "react";
import { DayPicker } from "react-day-picker";
import { useSearchParams, useNavigate } from "react-router-dom";
import "../styles/Filter.css";

const Filter = () => {
  const [isShown, setisShown] = useState<boolean>(false);

  const [search] = useSearchParams();
  const activeId = search.get("id");
  const navigate = useNavigate();

  const [selectedMonth, setSelectedMonth] = useState<Date | undefined>(
    () => new Date()
  );

  return (
    <>
      <div
        id="transaction-filter"
        className="btn-group"
        role="group"
        aria-label="Transaction filter"
      >
        <input
          type="radio"
          className="btn-check"
          name="bucket"
          id="bucket-0"
          checked={activeId === null}
          onChange={() => {
            navigate("/transactions");
            setisShown(false);
          }}
        />
        <label className="btn btn-outline-primary" htmlFor="bucket-0">
          All buckets
        </label>
        <input
          type="radio"
          className="btn-check"
          name="bucket"
          id="bucket-1"
          checked={activeId === "1"}
          onChange={() => {
            navigate("/transactions?id=1");
            setisShown(false);
          }}
        />
        <label className="btn btn-outline-primary" htmlFor="bucket-1">
          Salary Bucket
        </label>

        <input
          type="radio"
          className="btn-check"
          name="bucket"
          id="bucket-2"
          checked={activeId === "2"}
          onChange={() => {
            navigate("/transactions?id=2");
            setisShown(false);
          }}
        />
        <label className="btn btn-outline-primary" htmlFor="bucket-2">
          Groceries Bucket
        </label>
        <input
          type="radio"
          className="btn-check"
          name="bucket"
          id="bucket-3"
          checked={activeId === "3"}
          onChange={() => {
            navigate("/transactions?id=3");
            setisShown(false);
          }}
        />
        <label className="btn btn-outline-primary" htmlFor="bucket-3">
          Shopping Bucket
        </label>
        <input
          type="radio"
          className="btn-check"
          name="filter"
          id="filter"
          value="Filter on month"
          checked={activeId === "filter"}
          onClick={() => setisShown(!isShown)}
        />
      
      </div>

      {activeId != null && isShown === false ? (
        <div>
          <span id="daypicker">
            <DayPicker
              month={selectedMonth}
              onMonthChange={setSelectedMonth}
              captionLayout="dropdown"
              showOutsideDays={false}
              modifiers={{}}
            />
          </span>

          <input
            id="filter-btn"
            type="button"
            value="Filter"
            onClick={() => {
              navigate(
                `/transactions?month=${
                  selectedMonth?.getMonth() + 1
                }&year=${selectedMonth?.getFullYear()}&id=${activeId}`
              );
              setisShown(false);
            }}
          />
        </div>
      ) : (
        <p></p>
      )}
    </>
  );
};

export default Filter;
