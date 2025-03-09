import {useState} from 'react';
import {PrimeReactProvider} from "primereact/api";
import {Button} from 'primereact/button';
import {Route, Routes} from "react-router";
import Dashboard from "./pages/Dashboard";
import Bank from "./pages/Bank";
import NotFound from "./pages/NotFound";

function App() {
    const [count, setCount] = useState(0);

    return (
        <PrimeReactProvider>
            asasasas dfsd f
            <div className="card flex justify-content-center">
                <Button label={count.toString()} onClick={() => setCount((count) => count + 1)}/>
            </div>

            <Routes>
                <Route path="wallet">
                    <Route index element={<Dashboard/>}/>
                    <Route path="bank" element={<Bank/>}/>
                </Route>
                <Route path="*" element={<NotFound/>}/>
            </Routes>
        </PrimeReactProvider>
    );
}

export default App;
