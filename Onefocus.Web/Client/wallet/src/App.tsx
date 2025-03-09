import {useState} from 'react';
import {PrimeReactProvider} from "primereact/api";
import {Button} from 'primereact/button';

function App() {
    const [count, setCount] = useState(0);

    return (
        <PrimeReactProvider>
            asasasas dfsd f
            <div className="card flex justify-content-center">
                <Button label={count.toString()} onClick={() => setCount((count) => count + 1)}/>
            </div>
        </PrimeReactProvider>
    );
}

export default App;
