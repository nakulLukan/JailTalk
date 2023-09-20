window.Helper = {};

/*Open any given content in a new window without closing current window.*/
window.Helper.openInNewTab = (url, message) => {
    let newTab = window.open('', '_blank');
    newTab.document.write(message);
    newTab.location.href = url;
}