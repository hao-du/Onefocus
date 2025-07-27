import React from 'react';
import CurrencyList from '../components/CurrencyList';

const Currency = React.memo(() => {
    return <CurrencyList />;
});

export default Currency;