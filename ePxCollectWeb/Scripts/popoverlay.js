jQuery(document).ready(function($){
	//open the lateral panel
	$('#lbtn').on('click', function(event){
		event.preventDefault();
		$('.cd-panel-left').addClass('is-visible');
		$('#ldivcontent').html($('#secondary').html());
	});
	$('#rbtn').on('click', function (event) {
	    event.preventDefault();
	    $('.cd-panel-right').addClass('is-visible');
	    $('#rdivcontent').html($('#tertiary').html());
	});
	//clode the lateral panel
	$('.cd-panel-left').on('click', function(event){
		if( $(event.target).is('.cd-panel-left') || $(event.target).is('.cd-panel-close') ) { 
			$('.cd-panel-left').removeClass('is-visible');
			event.preventDefault();
		}
	});
	$('.cd-panel-right').on('click', function (event) {
	    if ($(event.target).is('.cd-panel-right') || $(event.target).is('.cd-panel-close')) {
	        $('.cd-panel-right').removeClass('is-visible');
	        event.preventDefault();
	    }
	});
});