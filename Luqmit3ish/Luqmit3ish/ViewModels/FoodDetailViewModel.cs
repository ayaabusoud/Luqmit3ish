<?xml version="1.0" encoding="UTF-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
             xmlns:local="clr-namespace:Luqmit3ish.Converter"
              xmlns:ffimageloading="clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms" 
             x:Class="Luqmit3ish.Views.FoodDetailPage"
             x:Name='FoodDetails'
             Title="Food Details"
             Shell.TabBarIsVisible="False"
             Style="{StaticResource ContentPageColor}">

    <ContentPage.Resources>
        <ResourceDictionary>
            <local:EqualOneConverter x:Key="equalOneConverter" />
            <local:NotEqualOneConverter x:Key="notEqualOneConverter" />
        </ResourceDictionary>
    </ContentPage.Resources>
    
    <ContentPage.Content>
        <Grid RowDefinitions="0.4*,0.6*" Margin="0">
            <ffimageloading:CachedImage LoadingPlaceholder="loading.PNG"
                                         Grid.Row="0" Source="{Binding DishInfo.Photo}" Aspect="AspectFill"/>
            <Frame  Grid.Row="1" HorizontalOptions="Fill" VerticalOptions="Fill" CornerRadius="50" 
                    Margin="-15,-60,-20,-40"  HasShadow="True"
                    Style="{StaticResource FrameColor}">
                <StackLayout Margin="10" HorizontalOptions="Fill"
                             Spacing="15" Padding="7">

                    <Label Text="{Binding  DishInfo.Restaurant.Name}" 
                           FontSize="Medium" Style="{StaticResource LabelColor}" Margin="0,0,0,-15">
                        <Label.GestureRecognizers>
                            <TapGestureRecognizer Command="{Binding ProfileCommand}"
                                                  CommandParameter="{Binding DishInfo.Restaurant}"  />
                        </Label.GestureRecognizers>
                    </Label>

                    <Grid ColumnDefinitions="*,auto" >
                        <Label Grid.Column="0" Text="{Binding  DishInfo.DishName}" 
                           FontSize="Title" Style="{StaticResource LabelColor}" FontAttributes="Bold"/>

                        <StackLayout  Orientation="Horizontal"
                                     Grid.Column="1" Margin="26,-5,0,0">
                            <Label Text="&#xf055;" FontFamily="FontAwesomeIcons" 
                                   FontSize="30" Margin="0,10,0,0" TextColor="{Binding PlusColor}">
                                <Label.GestureRecognizers>
                                    <TapGestureRecognizer CommandParameter="{Binding DishInfo.Quantity}" 
                                                          Command="{Binding PlusCommand}"/>
                                </Label.GestureRecognizers>
                            </Label>
                            <Label Text="{Binding Counter}" FontAttributes="Bold" 
                                   Margin="3,11,3,0" FontSize="19" Style="{StaticResource LabelColor}"/>
                            <Label Text="&#xf056;" FontFamily="FontAwesomeIcons" FontSize="30" 
                                   VerticalOptions="Center" Margin="0,10,0,0" TextColor="{Binding MinusColor}">
                                <Label.GestureRecognizers>
                                    <TapGestureRecognizer Command="{Binding MinusCommand}"/>
                                </Label.GestureRecognizers>
                            </Label>
                        </StackLayout>
                    </Grid>


                    <Label VerticalOptions="Start" Text="Description" Style="{StaticResource LabelColor}" FontSize="Medium" FontAttributes="Bold"/>
                    <Label VerticalOptions="Start" Style="{StaticResource LabelColor}" Text="{Binding DishInfo.Description}" Margin="0,-10,0,0" FontSize="16"/>
                    <StackLayout Orientation="Horizontal" Spacing="10">
                        <Label  Text="&#xf48b;" FontFamily="FontAwesomeIcons" FontSize="22" Margin="0,7,0,0" Style="{StaticResource LabelColor}"/>
                        <Label  FontSize="17" Style="{StaticResource LabelColor}" Margin="0,6,0,0" >
                            <Label.FormattedText>
                                <FormattedString>
                                    <Span Text="Opening Hours: " />
                                    <Span Text="{Binding DishInfo.Restaurant.OpeningHours}" Style="{StaticResource LabelColor}" FontAttributes="Bold" FontSize="17" />
                                </FormattedString>
                            </Label.FormattedText>
                        </Label>
                    </StackLayout>
                    <StackLayout Orientation="Horizontal" Spacing="12" Margin="3,0">
                        <Label Text="&#xf251;" FontFamily="FontAwesomeIcons" FontSize="20" Margin="0,5,0,0" Style="{StaticResource LabelColor}"/>
                        <StackLayout Orientation="Horizontal">
                            <Label FontSize="17" Style="{StaticResource LabelColor}" Margin="0,5,0,0">
                                <Label.FormattedText>
                                    <FormattedString>
                                        <Span Text="Keep valid for:  "  />
                                        <Span Text="{Binding DishInfo.KeepValid}"  FontAttributes="Bold" FontSize="17" />
                                    </FormattedString>
                                </Label.FormattedText>
                            </Label>
                            <Label FontSize="17" Style="{StaticResource LabelColor}" 
                                   FontAttributes="Bold" Margin="0,5,0,0" Text="Day" 
                                               IsVisible="{Binding DishInfo.KeepValid,Converter={StaticResource equalOneConverter}}" />
                            <Label FontSize="17" Style="{StaticResource LabelColor}" 
                                   FontAttributes="Bold" Margin="0,5,0,0" Text="Days" 
                                               IsVisible="{Binding DishInfo.KeepValid,Converter={StaticResource notEqualOneConverter}}" />
                        </StackLayout>

                    </StackLayout>
                    <StackLayout Orientation="Horizontal" Spacing="5">
                        <Label Text="&#xf562;" Margin="0,5,0,0" FontFamily="FontAwesomeIcons" FontSize="20"  Style="{StaticResource LabelColor}"/>
                        <StackLayout Orientation="Horizontal">
                            <Label Style="{StaticResource LabelColor}" Margin="0,5,0,0">
                                <Label.FormattedText>
                                    <FormattedString >
                                        <Span Text="Available quantity: " 
                                          FontSize="18"/>
                                        <Span Text="{Binding DishInfo.Quantity}" 
                                              FontAttributes="Bold" FontSize="Medium"/>
                                    </FormattedString>
                                </Label.FormattedText>
                            </Label>
                            <Label FontSize="Medium" FontAttributes="Bold"
                                               Text="Dish" Margin="0,5,0,0" Style="{StaticResource LabelColor}"
                                               IsVisible="{Binding DishInfo.Quantity,Converter={StaticResource equalOneConverter}}" />
                            <Label FontSize="Medium" FontAttributes="Bold"
                                               Text="Dishes" Margin="0,5,0,0" Style="{StaticResource LabelColor}"
                                               IsVisible="{Binding DishInfo.Quantity,Converter={StaticResource notEqualOneConverter}}" />
                        </StackLayout>

                    </StackLayout>


                    <Button CommandParameter="{Binding DishInfo.Id}" 
                                    Command="{Binding ReserveCommand}" 
                                    TextColor="White" Text="Reserve" CornerRadius="10"
                                    Margin="10,3,0,0" HeightRequest="50" 
                                    HorizontalOptions="Start" WidthRequest="800"
                                    BackgroundColor="{StaticResource PrimaryLight}" FontAttributes="Bold"  />

                </StackLayout>
            </Frame>
        </Grid>
    </ContentPage.Content>
</ContentPage>
