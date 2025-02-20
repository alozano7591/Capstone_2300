class Chargers extends React.Component {
    render() {
        return (
            <div className="text-center">
                <h1 className="display-4">Check out our chargers!</h1>
                <div className="games-container">
                    {chargers && chargers.map(charger => (
                        <div key={charger.chargerId}>
                            <Link to={`/charger/${charger.chargerId}`} className="game-card">
                                <h3>{charger.name}</h3>
                                <p>${charger.hourlyRate} per hour</p>
                                <p>{charger.description}</p>
                            </Link>
                            <hr />
                        </div>
                    ))}
                </div>
            </div>
        );
    }
}

console.log("Chargers.jsx is loaded");

ReactDOM.render(
    <Chargers />,
    document.getElementById('react-content-Chargers')
);
