$(".html-documentation-page .side-2 ul > li > a").on("click", function (e) {
  e.preventDefault();
})
$(".html-documentation-page a").on("click", function () {
  $(this).siblings('.sublist').toggle();
});
$(".html-documentation-page ul > li > a").on("click", function () {
  $(this).toggleClass('expand-category');
});

$(function () {
  $(".accordion-content-panel").accordion({ collapsible: true, active: true, heightStyle: "content" });
  $(".accordion-content-panel > h3").click();
  $(window).resize(function () {
    $(".ui-accordion-content").removeAttr("style")
  });
})