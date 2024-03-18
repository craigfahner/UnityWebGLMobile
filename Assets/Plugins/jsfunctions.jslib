mergeInto(LibraryManager.library, {

  GameOver: function () {
    var endCreditsElement = document.getElementById("endcredits");
    if (endCreditsElement) {
        endCreditsElement.style.display = "block";
    } else {
        console.error("Element with id 'endcredits' not found.");
    }
  },
});