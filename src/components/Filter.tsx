import { useSearchParams, useNavigate } from "react-router-dom";

const Filter = () => {
  const [search] = useSearchParams();
  const activeId = search.get("id");
  const navigate = useNavigate();

  return (
    <>
      <div className="btn-group" role="group" aria-label="Transaction filter">
        <input
          type="radio"
          className="btn-check"
          name="bucket"
          id="bucket-0"
          checked={activeId === null}
          onChange={() => navigate("/transactions")}
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
          onChange={() => navigate("/transactions?id=1")}
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
          onChange={() => navigate("/transactions?id=2")}
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
          onChange={() => navigate("/transactions?id=3")}
        />
        <label className="btn btn-outline-primary" htmlFor="bucket-3">
          Shopping Bucket
        </label>
      </div>
    </>
  );
};

export default Filter;
