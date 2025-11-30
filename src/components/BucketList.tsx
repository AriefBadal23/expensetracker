// import Bucket from "./Bucket";
import "../styles/BucketList.css";
import Bucket from "./Bucket";

const BucketList = () => {
  const buckets = [
    {
      name: "Salary",
      current: 500,
      icon: "💰",
    },
    {
      name: "Rent",
      current: 800,
      icon: "🏠",
    },
    {
      name: "Groceries",
      current: 150,
      icon: "🛒",
    },
    {
      name: "Transport",
      current: 60,
      icon: "🚌",
    },
    {
      name: "Savings",
      current: 300,
      icon: "📦",
    },
    {
      name: "Health",
      current: 120,
      icon: "💊",
    },
    {
      name: "Entertainment",
      current: 90,
      icon: "🎮",
    },
    {
      name: "Insurance",
      current: 70,
      icon: "🛡️",
    },
    {
      name: "Entertainment",
      current: 90,
      icon: "🎮",
    },
    {
      name: "Insurance",
      current: 70,
      icon: "🛡️",
    },
  ];

  return (
    <>
      <h1>Transaction Overview</h1>
      <div className="bucket-list">
        {buckets.map((b) => {
          return <Bucket name={b.name} amount={b.current} icon={b.icon} />;
        })}
      </div>
    </>
  );
};

export default BucketList;
// altijd een extra return wanneer je map() gebruikt;
