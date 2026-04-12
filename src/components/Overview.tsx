import BucketOverviewTable from "./BucketOverviewTable.tsx"
import Navbar from "./NavBar.tsx";
const Overview = () => {
    return (
        <div>
            <Navbar/>
            <h1>Month budget Overview</h1>
            <div className="container mt-4">
                <BucketOverviewTable/>
            </div>
        </div>
    )
}

export default Overview