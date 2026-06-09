let currentEmail = "";

async function registerUser() {

const username = prompt("Enter Username");

if (!username)
    return;

const email =
    document.getElementById("loginEmail").value;

const password =
    document.getElementById("loginPassword").value;

try {

    const response = await fetch(
        "/api/Auth/register",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                username: username,
                email: email,
                password: password
            })
        }
    );

    const result = await response.text();

    alert(result);

} catch (error) {

    console.error(error);
    alert("Register Failed");
}

}

async function login() {

const email =
    document.getElementById("loginEmail").value;

const password =
    document.getElementById("loginPassword").value;

try {

    const response = await fetch(
        "/api/Auth/login",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        }
    );

    if (!response.ok) {
        alert("Invalid Login");
        return;
    }

    const data = await response.json();

    currentEmail = email;

    document.getElementById("email").value =
        email;

    document.getElementById("balance").innerHTML =
        "Balance: ₹" + data.balance;

    alert("Login Successful");

    loadHistory();

} catch (error) {

    console.error(error);
    alert("Login Failed");
}

}

async function playSlot() {

const email =
    document.getElementById("email").value;

const betAmount =
    parseFloat(
        document.getElementById("bet").value
    );

try {

    const response = await fetch(
        "/api/Game/slot",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: email,
                betAmount: betAmount
            })
        }
    );

    const data =
        await response.json();

    document.getElementById("reels").innerHTML =
        data.reels;

    document.getElementById("result").innerHTML =
        data.result + " | ₹" +
        data.winnings;

    document.getElementById("balance").innerHTML =
        "Balance: ₹" +
        data.newBalance;

    loadHistory();

} catch (error) {

    console.error(error);
    alert("Game Error");
}

}

async function loadHistory() {

if (currentEmail === "")
    return;

try {

    const response = await fetch(
        "/api/Game/history/" +
        currentEmail
    );

    const history =
        await response.json();

    const tbody =
        document.querySelector(
            "#historyTable tbody"
        );

    tbody.innerHTML = "";

    history.forEach(item => {

        tbody.innerHTML += `
    <tr>
        <td>${item.gameType}</td>
        <td>${item.betAmount}</td>
        <td>${item.winAmount}</td>
    </tr>
`;
    });

} catch (error) {

    console.error(error);
}

}
