class HelloReact extends React.Component {
    render() {
        return (
            <div>
                <h1>Hello from React!</h1>
            </div>
        );
    }
}
console.log("HelloReact.jsx is loaded");
ReactDOM.render(
    <HelloReact />,
    document.getElementById('react-content')
);
